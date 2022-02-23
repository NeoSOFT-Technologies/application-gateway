using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerByName
{
    public class GetTransformerByNameQuery:IRequest<Response<GetTransformerByNameDto>>
    {
        public string TemplateName { get; set; }
        public Gateway Gateway { get; set; }
    }
}
