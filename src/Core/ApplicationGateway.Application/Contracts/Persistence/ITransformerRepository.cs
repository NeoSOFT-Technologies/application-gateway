using ApplicationGateway.Domain.TykData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
   public interface ITransformerRepository: IAsyncRepository<Transformers>
    {
        Task<Transformers> GetTransformerByName(string name);
    }
}
