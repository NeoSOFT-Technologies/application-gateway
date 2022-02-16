using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
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
using static ApplicationGateway.Domain.TykData.Key;

namespace ApplicationGateway.Infrastructure.KeyWrapper
{
    public class TykKeyService: IKeyService
    {
        private readonly ILogger<TykKeyService> _logger;
        private readonly FileOperator _fileOperator;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykKeyService(ILogger<TykKeyService> logger, FileOperator fileOperator, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _fileOperator = fileOperator;
            _tykConfiguration = tykConfiguration.Value;
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
            _logger.LogInformation($"GetKeyAsync completed for {keyId}");
            string transformedObj = await _fileOperator.Transform(keyResponse, "GetKeyTransformer");
            Key key = JsonConvert.DeserializeObject<Key>(transformedObj);

            JObject keyObj = JObject.Parse(keyResponse);

            #region Check & Add Poliicies, if exists
            if (keyObj["apply_policies"].HasValues)
            {
               foreach (var policy in keyObj["apply_policies"])
                {
                    key.Policies.Add(policy.ToString());
                }
            }
            #endregion

            #region Add accessRights in Key
            List<AccessRightsModel> accessRights = new List<AccessRightsModel>();   
            foreach(var accessRight in keyObj["access_rights"])
            {
                AccessRightsModel accessRightsModel = new AccessRightsModel();
                accessRightsModel.ApiId = Guid.Parse(accessRight["api_id"].ToString());
                accessRightsModel.ApiName = accessRight["api_name"].ToString();

                #region Add versions in accessRights
                List<string> versions = new List<string>();
                foreach(var version in accessRight["versions"])
                {
                    versions.Add(version.ToString());
                }
                accessRightsModel.Versions = versions;
                #endregion

                #region Add allowed urls in accessRights, if exists
                if (accessRight["allowed_urls"].HasValues)
                {
                    Key.AllowedUrl urls = new Key.AllowedUrl();
                    urls.Url = accessRight["allowed_urls"]["url"].ToString();
                    List<string> methods = new List<string>();
                    foreach(var method in accessRight["allowed_urls"]["url"])
                    {
                        methods.Add(method.ToString());
                    }
                    urls.Methods = methods;

                    accessRightsModel.AllowedUrls = urls;
                }
                #endregion
                accessRights.Add(accessRightsModel);
            }

            key.AccessRights = accessRights;
            #endregion
            return key;
        }

        public async Task<Key> CreateKeyAsync(Key key)
        {
            _logger.LogInformation($"CreateKeyAsync Initiated for {key}");
            string requestString = JsonConvert.SerializeObject(key);
            string transformedObj = await _fileOperator.Transform(requestString,"CreateKeyTransformer");
   
            JObject jsonObj = JObject.Parse(transformedObj);
            jsonObj["access_rights"] = new JObject();
            if (key.Policies.Any())
            {
                JArray policies = new JArray();
                key.Policies.ForEach(policy => policies.Add(policy));
                jsonObj["apply_policies"] = policies;
            }

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

            string keyResponse = await _restClient.PostAsync(jsonObj);
            JObject responseObj = JObject.Parse(keyResponse);

            key.KeyId = responseObj["key"].ToString();
            return key;
        }

        public async Task DeleteKeyAsync(string keyId)
        {
            _logger.LogInformation($"DeleteKeyAsync initiated for {keyId}");
            await _restClient.DeleteAsync(keyId);
            _logger.LogInformation($"DeleteKeyAsync completed for {keyId}");
        }


    }   
}
