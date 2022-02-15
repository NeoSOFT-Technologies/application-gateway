using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApplicationGateway.Infrastructure.ApiWrapper
{
    public class TykApiService : IApiService
    {
        private readonly ILogger<TykApiService> _logger;
        private readonly FileOperator _fileOperator;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykApiService(ILogger<TykApiService> logger, FileOperator fileOperator, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _fileOperator = fileOperator;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/apis", _headers);
        }

        public async Task<Api> CreateApiAsync(Api api)
        {
            _logger.LogInformation("CreateApiAsync Initiated with {@Api}", api);
            api.ApiId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(api);
            string transformed = await _fileOperator.Transform(requestJson, "CreateApiTransformer");

            #region Add ApiId to Api
            JObject transformedObject = JObject.Parse(transformed);
            transformedObject.Add("api_id", api.ApiId);
            #endregion

            await _restClient.PostAsync(transformedObject);

            _logger.LogInformation("CreateApiAsync Completed");
            return api;
        }

        public async Task<Api> UpdateApiAsync(Api api)
        {
            _logger.LogInformation("UpdateApiAsync Initiated with {@Api}", api);
            string inputJson = JsonConvert.SerializeObject(api);
            string transformed = await _fileOperator.Transform(inputJson, "UpdateApiTransformer");

            JObject inputObject = JObject.Parse(inputJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add version_data to API
            if (inputObject["Versions"].Count() != 0)
            {
                transformedObject["version_data"]["versions"] = new JObject();
                foreach (JToken version in inputObject["Versions"])
                {
                    (version as JObject).Add("use_extended_paths", true);
                    JArray removeGlobalHeaders = new JArray();
                    removeGlobalHeaders.Add("Authorization");
                    (version as JObject).Add("global_headers_remove", removeGlobalHeaders);
                    (transformedObject["version_data"]["versions"] as JObject).Add($"{version["Name"]}", version);
                    (transformedObject["version_data"]["versions"][$"{version["Name"]}"] as JObject).Add("override_target", version["OverrideTarget"]);
                }
            }
            #endregion

            #region Add OIDC Configuration to API
            if (inputObject["AuthType"].ToString() == "openid")
            {
                transformedObject["openid_options"]["providers"] = new JArray();
                foreach (JToken provider in inputObject["OpenidOptions"]["Providers"])
                {
                    JObject newProvider = new JObject();
                    newProvider.Add("issuer", provider["Issuer"]);
                    JObject newClient = new JObject();
                    foreach (JToken client in provider["Client_ids"])
                    {
                        string base64ClientId = Convert.ToBase64String(Encoding.UTF8.GetBytes(client["ClientId"].ToString()));
                        newClient.Add(base64ClientId, client["Policy"]);
                    }
                    newProvider.Add("client_ids", newClient);
                    (transformedObject["openid_options"]["providers"] as JArray).Add(newProvider);
                }
            }
            #endregion

            await _restClient.PutAsync(transformedObject);

            _logger.LogInformation("UpdateApiAsync Completed");
            return api;
        }

        public async Task DeleteApiAsync(Guid apiId)
        {
            _logger.LogInformation("UpdateApiAsync Initiated with {@Guid}", apiId);
            await _restClient.DeleteAsync(apiId.ToString());
            _logger.LogInformation("UpdateApiAsync Completed");
        }
    }
}
