using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper
{
    public interface IApiService
    {
        Task<List<Api>> GetAllApisAsync();
        Task<Api> GetApiByIdAsync(Guid apiId);
        Task<Api> CreateApiAsync(Api api);
        Task<Api> UpdateApiAsync(Api api);
        Task DeleteApiAsync(Guid apiId);
    }
}
