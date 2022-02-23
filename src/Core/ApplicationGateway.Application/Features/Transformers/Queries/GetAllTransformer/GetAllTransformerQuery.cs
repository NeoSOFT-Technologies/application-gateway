using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer
{
    public class GetAllTransformerQuery : IRequest<Response<IEnumerable<GetAllTransformerDto>>>
    {
    }
}
