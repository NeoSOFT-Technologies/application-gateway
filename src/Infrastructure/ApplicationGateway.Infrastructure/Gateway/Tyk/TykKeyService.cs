using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApplicationGateway.Domain.Entities.Key;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk

{
    public class TykKeyService: IKeyService
    {
        private readonly ILogger<TykKeyService> _logger;
        private readonly FileOperator _fileOperator;
        private readonly TykConfiguration _tykConfiguration;
        private readonly IBaseService _baseService;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykKeyService(ILogger<TykKeyService> logger, FileOperator fileOperator, IOptions<TykConfiguration> tykConfiguration, IBaseService baseService)
        {
            _logger = logger;
            _fileOperator = fileOperator;
            _tykConfiguration = tykConfiguration.Value;
            _baseService = baseService;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/keys", _headers);
        }

        public async Task<Key> GetKeyAsync(string keyId)
        {
            _logger.LogInformation($"GetKeyAsync initiated for {keyId}");
            var keyResponse = await _restClient.GetAsync(keyId);
            string transformedObj = await _fileOperator.Transform(keyResponse, "GetKeyTransformer");
            Key key = JsonConvert.DeserializeObject<Key>(transformedObj);

            JObject keyObj = JObject.Parse(keyResponse);

            #region Add Poliicies, if exists
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
                List<string> versions = new List<string>();
                foreach(var version in apiObj["versions"])
                {
                    versions.Add(version.ToString());
                }
                accessRightsModel.Versions = versions;
                #endregion

                #region Add allowed urls in accessRights, if exists
                if (apiObj["allowed_urls"].HasValues)
                {
                    AllowedUrl urls = new AllowedUrl();
                    urls.url = apiObj["allowed_urls"]["url"].ToString();
                    List<string> methods = new List<string>();
                    foreach(var method in accessRight["allowed_urls"]["url"])
                    {
                        methods.Add(method.ToString());
                    }
                    urls.methods = methods;

                    accessRightsModel.AllowedUrls = urls;
                }
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
            string transformedObj = await _fileOperator.Transform(requestString,"CreateKeyTransformer");
   
            JObject jsonObj = JObject.Parse(transformedObj);
            jsonObj["access_rights"] = new JObject();
            #region Add Policies, id exists
            if (key.Policies.Any())
            {
                JArray policies = new JArray();
                key.Policies.ForEach(policy => policies.Add(policy));
                jsonObj["apply_policies"] = policies;
                goto skipAccessRights;
            }
            #endregion

            #region Add AccessRights, if exists
            if (key.AccessRights.Any())
            {
                foreach (var api in key.AccessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);
                    JArray versions = new JArray();
                    api.Versions.ForEach(v => versions.Add(v));
                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["ApiId"]);
                    accObj.Add("api_name", obj["ApiName"]);
                    accObj.Add("versions", versions);
                    (jsonObj["access_rights"] as JObject).Add(obj["ApiId"].ToString(), accObj);
                }
            }
        #endregion

        skipAccessRights:
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
            string transformedObj = await _fileOperator.Transform(requestString, "UpdateKeyTransformer");


            JObject jsonObj = JObject.Parse(transformedObj);
            if (key.Policies.Any())
            {
                JArray policies = new JArray();
                key.Policies.ForEach(policy => policies.Add(policy));
                jsonObj["apply_policies"] = policies;
            }
            if (key.AccessRights.Any())
            {
                jsonObj["access_rights"] = new JObject();
                foreach (var api in key.AccessRights)
                {
                    string jsonString = JsonConvert.SerializeObject(api);
                    JObject obj = JObject.Parse(jsonString);
                    JArray versions = new JArray();
                    api.Versions.ForEach(v => versions.Add(v));
                    JObject accObj = new JObject();
                    accObj.Add("api_id", obj["ApiId"]);
                    accObj.Add("api_name", obj["ApiName"]);
                    accObj.Add("versions", versions);
                    (jsonObj["access_rights"] as JObject).Add(obj["ApiId"].ToString(), accObj);
                }
            }


            string keyResponse = await _restClient.PutKeyAsync(jsonObj,key.KeyId);
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

    }   
}
