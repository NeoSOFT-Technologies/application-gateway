using ApplicationGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories
{
    public interface IApiDtoRepository:IAsyncRepository<ApiDto>
    {
    }
}
