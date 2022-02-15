using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper
{
    public interface IApiService
    {
        Task<Api> CreateApiAsync(Api api);
        Task<Api> UpdateApiAsync(Api api);
        Task DeleteApiAsync(Guid apiId);
    }
}
