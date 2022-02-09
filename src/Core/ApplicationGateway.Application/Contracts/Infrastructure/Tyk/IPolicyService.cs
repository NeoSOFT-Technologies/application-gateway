namespace ApplicationGateway.Application.Contracts.Infrastructure.Tyk
{
    public interface IPolicyService
    {
        Task<string> CreatePolicy(string requestJson);
    }
}
