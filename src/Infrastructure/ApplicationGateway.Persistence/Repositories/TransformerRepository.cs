using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Persistence.Repositories
{
    public class TransformerRepository : BaseRepository<Transformer>, ITransformerRepository
    {
        private readonly ILogger _logger;
        public TransformerRepository(ApplicationDbContext dbContext, ILogger<Transformer> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public async Task<Transformer> GetTransformerByNameAndGateway(string name, Gateway gateway)
        {
            _logger.LogInformation("GetTransformerByNameAndGateway Initiated with {@string} & {@Gateway}", name, gateway);
            var transformer = _dbContext.Transformers.Where(a => a.TemplateName == name && a.Gateway == gateway).FirstOrDefault();
            _logger.LogInformation("GetTransformerByNameAndGateway Completed");
            return transformer;
        }
    }
}
