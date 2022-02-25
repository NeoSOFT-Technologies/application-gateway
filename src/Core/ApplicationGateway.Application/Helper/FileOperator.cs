using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Application.Helper
{
    public class FileOperator
    {
        public FileOperator()
        {

        }

        public async Task<string> ReadPolicies(string policiesFolderPath)
        {
            if (!Directory.Exists(policiesFolderPath))
            {
                Directory.CreateDirectory(policiesFolderPath);
            }
            if (!File.Exists($@"{policiesFolderPath}\policies.json"))
            {
                StreamWriter sw = File.CreateText($@"{policiesFolderPath}\policies.json");
                await sw.WriteLineAsync("{}");
                sw.Dispose();
            }
            return await File.ReadAllTextAsync($@"{policiesFolderPath}\policies.json");
        }

        public async Task WritePolicies(string policiesFolderPath, string content)
        {
            await File.WriteAllTextAsync($@"{policiesFolderPath}\policies.json", content);
        }

        public async Task CreatePolicy(string policiesFolderPath, string policyId, JObject transformedObject)
        {
            string policiesJson = await ReadPolicies(policiesFolderPath);

            JObject policiesObject = JObject.Parse(policiesJson);
            policiesObject.Add(policyId, transformedObject);

            await WritePolicies(policiesFolderPath, policiesObject.ToString());
        }

        public async Task UpdateDeletePolicyById(string policiesFolderPath, string policyId, JObject transformedObject = null)
        {
            string policiesJson = await ReadPolicies(policiesFolderPath);
            JObject policiesObject = JObject.Parse(policiesJson);

            if (!policiesObject.ContainsKey(policyId))
            {
                throw new NotFoundException($"Policy with id:", policyId);
            }

            policiesObject.Remove(policyId);
            if (transformedObject is not null)
                policiesObject.Add(policyId, transformedObject);

            await WritePolicies(policiesFolderPath, policiesObject.ToString());
        }
    }
}
