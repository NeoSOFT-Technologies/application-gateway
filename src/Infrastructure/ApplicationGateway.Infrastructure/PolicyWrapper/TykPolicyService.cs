using ApplicationGateway.Application.Contracts.Infrastructure.Tyk;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.TykData;
using JUST;
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

        public TykPolicyService(ILogger<TykPolicyService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
        }

        public async Task<Policy> CreatePolicy(Policy policy)
        {
            _logger.LogInformation("CreatePolicy Initiated");
            policy.PolicyId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(policy);
            string path = Directory.GetCurrentDirectory();
            string transformer = await File.ReadAllTextAsync(path + @"\JsonTransformers\Tyk\PolicyTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
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
            string policiesFolderPath = _tykConfiguration.PoliciesFolderPath;
            if (!Directory.Exists(policiesFolderPath))
            {
                Directory.CreateDirectory(policiesFolderPath);
            }
            if (!File.Exists(policiesFolderPath + @"\policies.json"))
            {
                StreamWriter sw = File.CreateText(policiesFolderPath + @"\policies.json");
                await sw.WriteLineAsync("{}");
                sw.Dispose();
            }
            string policiesJson = await File.ReadAllTextAsync(policiesFolderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            policiesObject.Add(policy.PolicyId.ToString(), transformedObject);

            await File.WriteAllTextAsync(policiesFolderPath + @"\policies.json", policiesObject.ToString());

            _logger.LogInformation("HotReload Completed");
            _logger.LogInformation("CreatePolicy Completed");
            return policy;
        }
    }
}
