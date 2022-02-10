using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper
{
    public interface IPolicyService
    {
        Task<Policy> CreatePolicy(Policy policy);
    }
}
