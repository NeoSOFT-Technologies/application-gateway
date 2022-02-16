using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Queries.GetTransformer
{
    public class GetTransformerQuery : IRequest<Response<IEnumerable<GetTransformerDto>>>
    {
    }
}
