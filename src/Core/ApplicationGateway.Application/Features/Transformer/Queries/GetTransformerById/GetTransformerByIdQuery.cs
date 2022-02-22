using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerById
{
    public class GetTransformerByIdQuery:IRequest<Response<GetTransformerByIdDto>>
    {
        public Guid Id { get; set; }
    }
}
