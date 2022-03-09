using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class KeyDtoRepository : BaseRepository<KeyDto>, IKeyDtoRepository
    {
        public KeyDtoRepository(ApplicationDbContext dbContext, ILogger<KeyDto> logger) : base(dbContext, logger)
        {
        }

        public override async Task UpdateAsync(KeyDto entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
            _dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
