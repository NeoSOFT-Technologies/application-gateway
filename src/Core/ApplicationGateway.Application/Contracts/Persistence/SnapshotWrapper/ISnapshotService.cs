using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Domain.TykData;

namespace ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper
{
    public interface ISnapshotService
    {
        Task<Snapshot> CreateSnapshot(Enums.Gateway gateway, Enums.Type type, Enums.Operation operation, string key, dynamic? dynamic);
    }
}
