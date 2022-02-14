using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper
{
    public interface IApiService
    {
        Task<Api> CreateApi(Api api);
    }
}
