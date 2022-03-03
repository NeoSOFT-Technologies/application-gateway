using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Application.Contracts.Infrastructure
{
    public interface IRedisService
    {
        Task PublishAsync(string message);
        Task<string> GetAsync(string policyId);
        Task CreateUpdateAsync(string policyId, JObject transformedObject, string operation);
        Task DeleteAsync(string policyId);
    }
}
