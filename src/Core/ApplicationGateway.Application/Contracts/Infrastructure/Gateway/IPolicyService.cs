using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Contracts.Infrastructure.Gateway
{
    public interface IPolicyService
    {
        Task<List<Policy>> GetAllPoliciesAsync();
        Task<Policy> GetPolicyByIdAsync(Guid policyId);
        Task<Policy> CreatePolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(Policy policy);
        Task DeletePolicyAsync(Guid policyId);
    }
}
