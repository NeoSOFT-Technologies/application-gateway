using ApplicationGateway.Application.Contracts.Infrastructure.Tyk;
using ApplicationGateway.Application.Models.Tyk;
using JUST;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Infrastructure.Tyk
{
    public class PolicyService : IPolicyService
    {
        private readonly TykConfiguration _tykConfiguration;
        private readonly ILogger<PolicyService> _logger;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;
        public PolicyService(ILogger<PolicyService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", "foo" }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
        }

        public async Task<string> CreatePolicy(string requestJson)
        {
            _logger.LogInformation("CreatePolicy Initiated");
            string path = Directory.GetCurrentDirectory();
            string transformer = await File.ReadAllTextAsync(path + @"\JsonTransformers\PolicyTransformer.json");
            string transformed = new JsonTransformer().Transform(transformer, requestJson);

            JObject inputObject = JObject.Parse(requestJson);
            JObject transformedObject = JObject.Parse(transformed);
            if (inputObject["APIs"].Count() != 0)
            {
                transformedObject["access_rights"] = new JObject();
                foreach (var api in inputObject["APIs"])
                {
                    var apiObject = new JObject()
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
                var sw = File.CreateText(policiesFolderPath + @"\policies.json");
                await sw.WriteLineAsync("{}");
                sw.Dispose();
            }
            string policiesJson = await File.ReadAllTextAsync(policiesFolderPath + @"\policies.json");
            JObject policiesObject = JObject.Parse(policiesJson);
            string policyId = Guid.NewGuid().ToString();
            policiesObject.Add(policyId, transformedObject);

            await File.WriteAllTextAsync(policiesFolderPath + @"\policies.json", policiesObject.ToString());

            await _restClient.GetAsync(null);

            _logger.LogInformation("CreatePolicy Completed");
            return policyId;
        }
    }
}
