using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class KeyRepository : BaseRepository<Key>, IKeyRepository
    {
        public KeyRepository(ApplicationDbContext dbContext, ILogger<Key> logger) : base(dbContext, logger)
        {
        }

        public override async Task UpdateAsync(Key entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
            _dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
