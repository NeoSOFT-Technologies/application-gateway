using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetTransformerById
{
    public class GetTransformerByIdQuery:IRequest<Response<GetTransformerByIdDto>>
    {
        public Guid TransformerId { get; set; }
    }
}
