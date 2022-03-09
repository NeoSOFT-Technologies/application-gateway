using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
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
    internal class PolicyDtoRepository : BaseRepository<PolicyDto>, IPolicyDtoRepository
    {
        public PolicyDtoRepository(ApplicationDbContext dbContext, ILogger<PolicyDto> logger) : base(dbContext, logger)
        {
        }

        public override async Task UpdateAsync(PolicyDto entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
            _dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
