using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using ApplicationGateway.Domain.TykData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
   public interface ITransformerRepository: IAsyncRepository<Transformer>
    {
        Task<Transformer> CreateTransformer(string name, string templateTranformer, Enums.Gateway gateway);
        Task<Transformer> GetTransformerByNameAndGateway(string name,string gateway);
    }
}
