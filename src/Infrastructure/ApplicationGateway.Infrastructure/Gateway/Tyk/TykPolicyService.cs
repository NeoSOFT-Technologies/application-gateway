using ApplicationGateway.Application.Contracts.Infrastructure;
using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk
{
    public class TykPolicyService : IPolicyService
    {
        private readonly TykConfiguration _tykConfiguration;
        private readonly ILogger<TykPolicyService> _logger;
        private readonly FileOperator _fileOperator;
        private readonly TemplateTransformer _templateTransformer;
        private readonly IRedisService _redisService;

        public TykPolicyService(ILogger<TykPolicyService> logger, IOptions<TykConfiguration> tykConfiguration, FileOperator fileOperator, TemplateTransformer templateTransformer, IRedisService redisService)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _fileOperator = fileOperator;
            _templateTransformer = templateTransformer;
            _redisService = redisService;
        }

        public async Task<List<Policy>> GetAllPoliciesAsync()
        {
            _logger.LogInformation("GetAllPoliciesAsync Initiated");
            string policiesJson = await _fileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            List<Policy> policies = new List<Policy>();

            #region Transform individual policy
            foreach (KeyValuePair<string, JToken> policy in policiesObject)
            {
                string transformed = await _templateTransformer.Transform(policy.Value.ToString(), TemplateHelper.GETPOLICY_TEMPLATE, Domain.Entities.Gateway.Tyk);
                JObject transformedObject = JObject.Parse(transformed);
                transformedObject["policyId"] = policy.Key;

                transformedObject = GetPolicyApis(policy.Value["access_rights"] as JObject, transformedObject);
                
                Policy transformedtPolicy = JsonConvert.DeserializeObject<Policy>(transformedObject.ToString());
                policies.Add(transformedtPolicy);
            }
            #endregion

            _logger.LogInformation("GetAllPoliciesAsync Completed");
            return policies;
        }

        public async Task<Policy> GetPolicyByIdAsync(Guid policyId)
        {
            _logger.LogInformation("GetPolicyByIdAsync Initiated with {@Guid}", policyId);

            #region Transform policy
            string policyJson = await _redisService.GetAsync(policyId.ToString());
            JObject policyObject = JObject.Parse(policyJson);
            string transformed = await _templateTransformer.Transform(policyJson, TemplateHelper.GETPOLICY_TEMPLATE, Domain.Entities.Gateway.Tyk);
            JObject transformedObject = JObject.Parse(transformed);
            transformedObject["policyId"] = policyId;
            transformedObject = GetPolicyApis(policyObject["access_rights"] as JObject, transformedObject);
            #endregion

            Policy policy = JsonConvert.DeserializeObject<Policy>(transformedObject.ToString());

            _logger.LogInformation("GetPolicyByIdAsync Completed");
            return policy;
        }

        public async Task<Policy> CreatePolicyAsync(Policy policy)
        {
            _logger.LogInformation("CreatePolicyAsync Initiated with {@Policy}", policy);

            policy.PolicyId = Guid.NewGuid();

            JObject transformedObject = await CreateUpdatePolicy(policy);

            #region Add Policy to policies.json
            await _redisService.CreateUpdateAsync(policy.PolicyId.ToString(), transformedObject, "create");
            #endregion

            _logger.LogInformation("CreatePolicyAsync Completed");
            return policy;
        }

        public async Task<Policy> UpdatePolicyAsync(Policy policy)
        {
            _logger.LogInformation("UpdatePolicyAsync Initiated with {@Policy}", policy);

            JObject transformedObject = await CreateUpdatePolicy(policy);

            #region Add Policy to policies.json
            await _redisService.CreateUpdateAsync(policy.PolicyId.ToString(), transformedObject, "update");
            #endregion

            _logger.LogInformation("UpdatePolicyAsync Completed");
            return policy;
        }

        public async Task DeletePolicyAsync(Guid policyId)
        {
            _logger.LogInformation("DeletePolicyAsync Initiated with {@Guid}", policyId);
            await _redisService.DeleteAsync(policyId.ToString());
            _logger.LogInformation("DeletePolicyAsync Completed");
        }

        private static JObject SetPolicyApis(JObject inputObject)
        {
            JObject jObject = new JObject();
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
                jObject.Add($"{api["Id"]}", apiObject);
            }
            return jObject;
        }

        private static JObject GetPolicyApis(JObject policy, JObject transformedObject)
        {
            foreach (KeyValuePair<string, JToken> api in policy)
            {
                JObject apiObject = JObject.Parse(api.Value.ToString());
                JObject transformedApiObject = new JObject()
                    {
                        { "Id", apiObject["api_id"] },
                        { "Name", apiObject["api_name"] },
                        { "Versions", apiObject["versions"] },
                        { "AllowedUrls", apiObject["allowed_urls"] },
                        { "Limit", apiObject["limit"] }
                    };
                (transformedObject["APIs"] as JArray).Add(transformedApiObject);
            }
            return transformedObject;
        }

        private async Task<JObject> CreateUpdatePolicy(Policy policy)
        {
            string requestJson = JsonConvert.SerializeObject(policy);
            string transformed = await _templateTransformer.Transform(requestJson, TemplateHelper.POLICY_TEMPLATE, Domain.Entities.Gateway.Tyk);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add Access Rights to Policy
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = SetPolicyApis(inputObject);
            }
            #endregion

            return transformedObject;
        }
    }
}