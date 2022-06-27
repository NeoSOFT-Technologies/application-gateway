using ApplicationGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IApiRepository:IAsyncRepository<Api>
    {
        Task<IReadOnlyList<Api>> GetSearchedResponseAsync(int page, int size, string col, string value);
    }
}
