using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper
{
    public interface IPolicyService
    {
        Task<Policy> CreatePolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(Policy policy);
    }
}
