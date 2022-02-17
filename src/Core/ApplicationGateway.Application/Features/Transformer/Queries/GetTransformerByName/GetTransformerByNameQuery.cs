using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Queries.GetTransformerByName
{
    public class GetTransformerByNameQuery:IRequest<Response<GetTransformerByNameDto>>
    {
        public string TemplateName { get; set; }
    }
}
