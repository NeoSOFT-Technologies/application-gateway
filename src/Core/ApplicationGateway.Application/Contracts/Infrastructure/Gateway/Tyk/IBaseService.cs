namespace ApplicationGateway.Application.Contracts.Infrastructure.Gateway.Tyk
{
    public interface IBaseService
    {
        Task HotReload();
    }
}
