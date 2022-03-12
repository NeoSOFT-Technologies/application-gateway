using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IKeyRepository:IAsyncRepository<Key>
    {
    }
}
