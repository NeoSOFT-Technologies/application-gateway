using ApplicationGateway.Domain.Entities;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IMessageRepository : IAsyncRepository<Message>
    {
        public Task<Message> GetMessage(string Code, string Lang);
    }
}
