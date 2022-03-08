using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class KeyDtoRepository : BaseRepository<KeyDto>, IKeyDtoRepository
    {
        public KeyDtoRepository(ApplicationDbContext dbContext, ILogger<KeyDto> logger) : base(dbContext, logger)
        {
        }
    }
}
