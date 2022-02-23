using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories
{
    public class TransformerRepository : BaseRepository<Transformer>, ITransformerRepository
    {
        private readonly ILogger _logger;
        public TransformerRepository(ApplicationDbContext dbContext, ILogger<Transformer> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }

        public async Task<Transformer> CreateTransformer(string name, string templateTranformer, Enums.Gateway gateway)
        {
            Transformer transformer = new Transformer()
            {
                TemplateName = name,
                TransformerTemplate = templateTranformer,
                Gateway = gateway.ToString()
            };
            var tran = await _dbContext.AddAsync(transformer);
            await _dbContext.SaveChangesAsync();
            return transformer;
        }

        public async Task<Transformer> GetTransformerByNameAndGateway(string name,string gateway)
        {
            _logger.LogInformation("GetTransformer Initiated");
            var transformer =   _dbContext.Transformers.Where(a => a.TemplateName == name && a.Gateway == gateway).FirstOrDefault();
            return transformer;
        }
    }
}
