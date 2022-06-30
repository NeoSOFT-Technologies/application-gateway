using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Exceptions;
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
        public async Task<(IEnumerable<Key> list, int count)> GetSearchedResponseAsync(int page, int size, string col, string value, string sortParam = null, bool isDesc = false)
        {
            string name = ValidateParam(col);
            Func<Key, bool> exp = null;
            switch (name)
            {
                case "KeyName":
                    exp = prop => prop.KeyName.Contains(value, StringComparison.InvariantCultureIgnoreCase);
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
                case "Expires":
                    exp = prop => prop.Expires.Equals(DateTime.Parse(value));
                    break;
                case "Policies":
                    exp = prop => prop.Policies.Contains(value);
                    break;
                default:
                    throw new BadRequestException($"{name} is invalid");
            }
            (IEnumerable<Key> response, int count) = await GetPagedListAsync(page: page, size: size, sortParam: sortParam, isDesc: isDesc, expression: exp);
            return (list: response, count: count);
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
