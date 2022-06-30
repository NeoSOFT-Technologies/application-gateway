using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class ApiRepository : BaseRepository<Api>, IApiRepository
    {
        public ApiRepository(ApplicationDbContext dbContext, ILogger<Api> logger) : base(dbContext, logger)
        {
        }

        public async Task<(IEnumerable<Api> list,int count)> GetSearchedResponseAsync(int page, int size, string col, string value, string sortParam = null, bool isDesc = false)
        {
            string name = ValidateParam(col);
            Func<Api, bool> exp = null;
            switch(name)
            {
                case "Name":
                    exp = prop => prop.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case "IsActive":
                    bool isActive;
                    if (value.ToLower() == "true")
                        isActive = true;
                    else if (value.ToLower() == "false")
                        isActive = false;
                    else
                        throw new BadRequestException($"{value} is not a boolean value");
                    exp = prop => prop.IsActive == isActive;
                    break;
                case "AuthType":
                    exp = prop => prop.AuthType.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case "TargetUrl":
                    exp = prop => prop.TargetUrl.Contains(value, StringComparison.InvariantCultureIgnoreCase);
                    break;
                default:
                        throw new BadRequestException($"{name} is invalid");
            }
            (IEnumerable<Api> response, int count) = await GetPagedListAsync( page:page,size:size,sortParam:sortParam,isDesc:isDesc,expression:exp);
            return (list:response, count:count);
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
