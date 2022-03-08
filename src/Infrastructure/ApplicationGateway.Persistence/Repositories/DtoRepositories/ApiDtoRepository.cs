using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence.Repositories.DtoRepositories
{
    internal class ApiDtoRepository : BaseRepository<ApiDto>, IApiDtoRepository
    {
        public ApiDtoRepository(ApplicationDbContext dbContext, ILogger<ApiDto> logger) : base(dbContext, logger)
        {
        }
    }
}
