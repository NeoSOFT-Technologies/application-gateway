using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Exceptions;
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
        private readonly IBaseService _baseService;
        private readonly TykConfiguration _tykConfiguration;
        private readonly ILogger<TykPolicyService> _logger;
        private readonly FileOperator _fileOperator;

        public TykPolicyService(IBaseService baseService, ILogger<TykPolicyService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _baseService = baseService;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _fileOperator = new FileOperator();
        }

        public async Task<Policy> CreatePolicyAsync(Policy policy)
        {
            _logger.LogInformation("CreatePolicyAsync Initiated with {@Policy}", policy);

            policy.PolicyId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(policy);
            string transformed = await _fileOperator.Transform(requestJson, "PolicyTransformer");

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add Access Rights to Policy
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = SetPolicyApis(inputObject);
            }
            #endregion

            #region Add Policy to policies.json
            string policiesJson = await FileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);
            policiesObject.Add(policy.PolicyId.ToString(), transformedObject);

            await FileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
            #endregion

            await _baseService.HotReload();

            _logger.LogInformation("CreatePolicyAsync Completed");
            return policy;
        }

        public async Task<Policy> UpdatePolicyAsync(Policy policy)
        {
            _logger.LogInformation("UpdatePolicyAsync Initiated with {@Policy}", policy);
            string requestJson = JsonConvert.SerializeObject(policy);
            string transformed = await _fileOperator.Transform(requestJson, "PolicyTransformer");

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);

            #region Add Access Rights to Policy
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = SetPolicyApis(inputObject);
            }
            #endregion

            #region Update Policy in policies.json
            string policiesJson = await FileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            string policyId = policy.PolicyId.ToString();
            if (!policiesObject.ContainsKey(policyId))
            {
                throw new NotFoundException($"Policy with id:", policyId);
            }

            policiesObject.Remove(policyId);
            policiesObject.Add(policyId, transformedObject);

            await FileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
            #endregion

            await _baseService.HotReload();

            _logger.LogInformation("UpdatePolicyAsync Completed");
            return policy;
        }

        public async Task DeletePolicyAsync(Guid policyId)
        {
            _logger.LogInformation("DeletePolicyAsync Initiated with {@Guid}", policyId);
            string policiesJson = await FileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                throw new NotFoundException($"Policy with id:", policyId.ToString());
            }

            policiesObject.Remove(policyId.ToString());
            await FileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());

            await _baseService.HotReload();

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
    }
}