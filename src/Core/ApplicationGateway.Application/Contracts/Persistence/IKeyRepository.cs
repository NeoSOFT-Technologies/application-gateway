using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IKeyRepository:IAsyncRepository<Key>
    {
        Task<IReadOnlyList<Key>> GetSearchedResponseAsync(int page, int size, string col, string value);
    }
}
