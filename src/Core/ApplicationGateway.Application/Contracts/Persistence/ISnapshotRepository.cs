using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface ISnapshotRepository : IAsyncRepository<Snapshot>
    {
        Task<Snapshot> TakeSnapshot(Snapshot snapshot, Enums.Operation operation);
    }
}
