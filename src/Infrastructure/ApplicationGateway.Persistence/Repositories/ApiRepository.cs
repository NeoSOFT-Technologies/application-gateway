using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class ApiRepository : BaseRepository<Api>, IApiRepository
    {
        public ApiRepository(ApplicationDbContext dbContext, ILogger<Api> logger) : base(dbContext, logger)
        {
        }

        public override async Task UpdateAsync(Api entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
            _dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
