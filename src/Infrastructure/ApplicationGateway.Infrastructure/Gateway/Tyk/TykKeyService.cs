using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.GatewayCommon;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk

{
    [ExcludeFromCodeCoverage]
    public class TykKeyService: IKeyService
    {
        private readonly ILogger<TykKeyService> _logger;
        private readonly TemplateTransformer _templateTransformer;
        private readonly TykConfiguration _tykConfiguration;
        private readonly IBaseService _baseService;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykKeyService(ILogger<TykKeyService> logger, TemplateTransformer templateTransformer, IOptions<TykConfiguration> tykConfiguration, IBaseService baseService)
        {
            _logger = logger;
            _templateTransformer = templateTransformer;
            _tykConfiguration = tykConfiguration.Value;
            _baseService = baseService;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/keys", _headers);
        }
        public async Task<List<string>> GetAllKeysAsync()
        {
            _logger.LogInformation("GetAllKeysAsync initiated");
            var listObj = JsonConvert.DeserializeObject<JObject>(await _restClient.GetAsync(null));
            #region Parse in List<string>
            JArray list = JArray.Parse(listObj["keys"].ToString());
            if (!list.Any())
                throw new NotFoundException("Any Key","");
            List<string> listKey = new List<string>();
            foreach (var keyId in list)
                listKey.Add(keyId.ToString());
            #endregion
            _logger.LogInformation("GetAllKeysAsync completed");
            return listKey;
        }
        public async Task<Key> GetKeyAsync(string keyId)
        {
            _logger.LogInformation($"GetKeyAsync initiated for {keyId}");
            var keyResponse = await _restClient.GetAsync(keyId);
            string transformedObj = await _templateTransformer.Transform(keyResponse, TemplateHelper.GETKEY_TEMPLATE, Domain.Entities.Gateway.Tyk);
            Key key = JsonConvert.DeserializeObject<Key>(transformedObj);

            JObject keyObj = JObject.Parse(keyResponse);

            #region Add Policies, if exists
            if (keyObj["apply_policies"].HasValues)
            {
                List<string> policies = new List<string>();
               foreach (var policy in keyObj["apply_policies"] as JArray)
                {
                    policies.Add((string)policy);
                }
                key.Policies = policies;
            }
            #endregion

            #region Add accessRights in Key
            List<AccessRightsModel> accessRights = new List<AccessRightsModel>();   
            foreach(var accessRight in keyObj["access_rights"])
            {
                AccessRightsModel accessRightsModel = new AccessRightsModel();
                var apiObj = accessRight.First;
                accessRightsModel.ApiId = Guid.Parse(apiObj["api_id"].ToString());
                accessRightsModel.ApiName = apiObj["api_name"].ToString();

                #region Add versions in accessRights
                accessRightsModel.Versions = ParseVersions(JArray.Parse(apiObj["versions"].ToString()));
                #endregion

                #region Add allowed urls in accessRights, if exists
                if (apiObj["allowed_urls"].HasValues)       
                    accessRightsModel.AllowedUrls = ParseAllowedUrl(JArray.Parse(apiObj["allowed_urls"].ToString()));
                
                #endregion

                #region Add Api Limit in accessRights, if exists
                if (apiObj["limit"].HasValues) 
                    accessRightsModel.Limit = ParseApiLimit((JObject)apiObj["limit"]);
                #endregion
                accessRights.Add(accessRightsModel);
            }

            key.AccessRights = accessRights;
            #endregion
            key.KeyId = keyId;
            _logger.LogInformation($"GetKeyAsync completed for {keyId}");
            return key;
        }

        public async Task<Key> CreateKeyAsync(Key key)
        {
            _logger.LogInformation($"CreateKeyAsync Initiated for {key}");
            string requestString = JsonConvert.SerializeObject(key);
            string transformedObj = await _templateTransformer.Transform(requestString, TemplateHelper.CREATEKEY_TEMPLATE, Domain.Entities.Gateway.Tyk);
   
            JObject jsonObj = JObject.Parse(transformedObj);
            jsonObj["access_rights"] = new JObject();
            jsonObj = await CreateUpdateKey(key, jsonObj);

            string keyResponse = await _restClient.PostAsync(jsonObj);
            await _baseService.HotReload();
            JObject responseObj = JObject.Parse(keyResponse);

            key.KeyId = responseObj["key"].ToString();
            return key;
        }

        public async Task<Key> UpdateKeyAsync(Key key)
        {
            _logger.LogInformation($"UpdateKeyAsync initiated for {key}");

            string requestString = JsonConvert.SerializeObject(key);
            string transformedObj = await _templateTransformer.Transform(requestString, TemplateHelper.UPDATEKEY_TEMPLATE, Domain.Entities.Gateway.Tyk);


            JObject jsonObj = JObject.Parse(transformedObj);
            jsonObj = await CreateUpdateKey(key, jsonObj);

            await _restClient.PutKeyAsync(jsonObj, key.KeyId);
            await _baseService.HotReload();
            _logger.LogInformation($"UpdateKeyAsync completed for {key}");
            return key;
        }
        public async Task DeleteKeyAsync(string keyId)
        {
            _logger.LogInformation($"DeleteKeyAsync initiated for {keyId}");
            await _restClient.DeleteAsync(keyId);
            await _baseService.HotReload();
            _logger.LogInformation($"DeleteKeyAsync completed for {keyId}");
        }

        private ApiLimit ParseApiLimit(JObject json)
        {
            ApiLimit limit = new ApiLimit()
            {
                Rate = (int)json["rate"],
                Per = (int)json["per"],
                Throttle_interval = (int)json["throttle_interval"],
                Throttle_retry_limit = (int)json["throttle_retry_limit"],
                Max_query_depth = (int)json["max_query_depth"],
                Quota_max = (int)json["quota_max"],
                Quota_remaining = (int)json["quota_remaining"],
                Quota_renews = (int)json["quota_renews"],
                Quota_renewal_rate = (int)json["quota_renewal_rate"],
            };
            return limit;
        }

        private List<AllowedUrl> ParseAllowedUrl(JArray json)
        {
            List<AllowedUrl> allowedUrls = new List<AllowedUrl>();
            foreach (var urlArray in json)
            {
                AllowedUrl urls = new AllowedUrl();
                urls.url = urlArray["url"].ToString();
                List<string> methods = new List<string>();
                foreach (var method in urlArray["methods"])
                {
                    methods.Add(method.ToString());
                }
                urls.methods = methods;
                allowedUrls.Add(urls);
            }

            return allowedUrls;
        }

        private List<string> ParseVersions(JArray json)
        {
            List<string> versions = new List<string>();
            foreach (var version in json)
            {
                versions.Add(version.ToString());
            }
            return versions;
        }

        private async Task<JObject> CreateUpdateKey(Key key, JObject jsonObj)
        {
            #region Add Policies, id exists
            if (key.Policies.Any())
            {
                JArray policies = new JArray();
                key.Policies.ForEach(policy => policies.Add(policy));
                jsonObj["apply_policies"] = policies;
            }
            #endregion

            #region Add AccessRights, if exists
            else if (key.AccessRights.Any())
            {
                foreach (var api in key.AccessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);

                    #region Add version details
                    JArray versions = new JArray();
                    api.Versions.ForEach(v => versions.Add(v));
                    #endregion

                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["ApiId"]);
                    accObj.Add("api_name", obj["ApiName"]);
                    accObj.Add("versions", versions);

                    #region Added allowedUrls, if exists
                    if (api.AllowedUrls.Any())
                    {
                        JArray allowedUrls = new JArray();
                        foreach (var urlItem in api.AllowedUrls)
                        {
                            JObject urlDetails = new JObject();
                            JArray methods = new JArray();
                            urlItem.methods.ForEach(method => methods.Add(method));
                            urlDetails.Add("url", urlItem.url);
                            urlDetails.Add("methods", methods);
                            allowedUrls.Add(urlDetails);
                        }
                        accObj.Add("allowed_urls", allowedUrls);
                    }
                    #endregion
                    #region Added perApiLimits, if exists
                    if (api.Limit != null)
                    {
                        JObject limit = new JObject();
                        limit.Add("rate", api.Limit.Rate);
                        limit.Add("per", api.Limit.Per);
                        limit.Add("throttle_interval", api.Limit.Throttle_interval);
                        limit.Add("throttle_retry_limit", api.Limit.Throttle_retry_limit);
                        limit.Add("max_query_depth", api.Limit.Max_query_depth);
                        limit.Add("quota_max", api.Limit.Quota_max);
                        limit.Add("quota_renews", api.Limit.Quota_renews);
                        limit.Add("quota_remaining", api.Limit.Quota_remaining);
                        limit.Add("quota_renewal_rate", api.Limit.Quota_renewal_rate);

                        accObj.Add("limit", limit);
                        accObj.Add("allowance_scope", api.ApiId.ToString());
                    }
                    #endregion

                    (jsonObj["access_rights"] as JObject).Add(obj["ApiId"].ToString(), accObj);
                }
            }
            #endregion

            return jsonObj;
        }
    }   
}
