using ApplicationGateway.Domain.Entities;

namespace ApplicationGateway.Application.Contracts.Persistence
{
   public interface ITransformerRepository: IAsyncRepository<Transformer>
    {
        Task<Transformer> GetTransformerByNameAndGateway(string name, Gateway gateway);
    }
}
