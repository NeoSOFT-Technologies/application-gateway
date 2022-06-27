using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories
{
    internal class PolicyRepository : BaseRepository<Policy>, IPolicyRepository
    {
        public PolicyRepository(ApplicationDbContext dbContext, ILogger<Policy> logger) : base(dbContext, logger)
        {
        }
        public async Task<IEnumerable<Policy>> GetSearchedResponseAsync(int page, int size, string col, string value, string sortParam = null, bool isDesc = false)
        {
            string name = ValidateParam(col);
            Func<Policy, bool> exp = null;
            switch (name)
            {
                case "Name":
                    exp = prop => prop.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case "AuthType":
                    exp = prop => prop.AuthType.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case "State":
                    exp = prop => prop.State.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case "Apis":
                    exp = prop => prop.Apis.Contains(value);
                    break;
                default:
                    throw new BadRequestException($"{name} is invalid");
            }
            var response = await GetPagedListAsync(page: page, size: size, sortParam: sortParam, isDesc: isDesc, expression: exp);
            return response;
        }
        public override async Task UpdateAsync(Policy entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).Property(p => p.CreatedDate).IsModified = false;
            _dbContext.Entry(entity).Property(p => p.CreatedBy).IsModified = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}
