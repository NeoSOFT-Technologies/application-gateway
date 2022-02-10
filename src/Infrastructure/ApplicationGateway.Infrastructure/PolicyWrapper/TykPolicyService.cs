using ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Infrastructure.PolicyWrapper
{
    public class TykPolicyService : IPolicyService
    {
        private readonly TykConfiguration _tykConfiguration;
        private readonly ILogger<TykPolicyService> _logger;
        private readonly FileOperator _fileOperator;

        public TykPolicyService(ILogger<TykPolicyService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _fileOperator = new FileOperator();
        }

        public async Task<Policy> CreatePolicy(Policy policy)
        {
            _logger.LogInformation("CreatePolicy Initiated with {@Policy}", policy);

            policy.PolicyId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(policy);
            string transformed = await _fileOperator.Transform(requestJson, "PolicyTransformer");

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add Access Rights to Policy
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = new JObject();
                foreach (JToken api in inputObject["APIs"])
                {
                    JObject apiObject = new JObject()
                    {
                        { "api_id", api["Id"] },
                        { "api_name", api["Name"] },
                        { "versions", api["Versions"] },
                        { "allowed_urls", api["AllowedUrls"] },
                        { "limit", api["Limit"] }
                    };
                    (transformedObject["access_rights"] as JObject).Add($"{api["Id"]}", apiObject);
                }
            }
            #endregion

            #region Add Policy to policies.json
            string policiesJson = await FileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);
            policiesObject.Add(policy.PolicyId.ToString(), transformedObject);

            await FileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
            #endregion

            _logger.LogInformation("HotReload Completed");
            _logger.LogInformation("CreatePolicy Completed");
            return policy;
        }
    }
}
