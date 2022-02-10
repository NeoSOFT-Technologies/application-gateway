using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.Tyk
{
    public interface IPolicyService
    {
        Task<Policy> CreatePolicy(Policy policy);
    }
}
