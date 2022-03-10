using ApplicationGateway.Domain.GatewayCommon;

namespace ApplicationGateway.Application.Contracts.Infrastructure.Gateway
{
    public interface IKeyService
    {
        Task<List<string>> GetAllKeysAsync();
        Task<Key> GetKeyAsync(string keyId);
        Task<Key> CreateKeyAsync(Key key);
        Task<Key> UpdateKeyAsync(Key key);
        Task DeleteKeyAsync(string keyId);
    }
}
