﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
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

        public TykPolicyService(IBaseService baseService, ILogger<TykPolicyService> logger, IOptions<TykConfiguration> tykConfiguration, FileOperator fileOperator)
        {
            _baseService = baseService;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _fileOperator = fileOperator;
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
                string transformed = await _fileOperator.Transform(policy.Value.ToString(), "GetPolicyTransformer");
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
            string policiesJson = await _fileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);
            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                throw new NotFoundException("Policy with id", policyId);
            }

            #region Transform policy
            string policyJson = policiesObject[policyId.ToString()].ToString();
            JObject policyObject = JObject.Parse(policyJson);
            string transformed = await _fileOperator.Transform(policyJson, "GetPolicyTransformer");
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
            string policiesJson = await _fileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);
            policiesObject.Add(policy.PolicyId.ToString(), transformedObject);

            await _fileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
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
            string policiesJson = await _fileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            string policyId = policy.PolicyId.ToString();
            if (!policiesObject.ContainsKey(policyId))
            {
                throw new NotFoundException($"Policy with id:", policyId);
            }

            policiesObject.Remove(policyId);
            policiesObject.Add(policyId, transformedObject);

            await _fileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
            #endregion

            await _baseService.HotReload();

            _logger.LogInformation("UpdatePolicyAsync Completed");
            return policy;
        }

        public async Task DeletePolicyAsync(Guid policyId)
        {
            _logger.LogInformation("DeletePolicyAsync Initiated with {@Guid}", policyId);
            string policiesJson = await _fileOperator.ReadPolicies(_tykConfiguration.PoliciesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            if (!policiesObject.ContainsKey(policyId.ToString()))
            {
                throw new NotFoundException($"Policy with id:", policyId.ToString());
            }

            policiesObject.Remove(policyId.ToString());
            await _fileOperator.WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());

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
    }
}