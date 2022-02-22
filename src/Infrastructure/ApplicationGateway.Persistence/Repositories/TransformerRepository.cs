using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories
{
    public class TransformerRepository : BaseRepository<Transformers>, ITransformerRepository
    {
        private readonly ILogger _logger;
        public TransformerRepository(ApplicationDbContext dbContext, ILogger<Transformers> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public async Task<Transformers> GetTransformerByName(string name)
        {
            var transformer =   _dbContext.Transformers.Where(a => a.TemplateName == name).FirstOrDefault();
            return transformer;
        }
    }
}
