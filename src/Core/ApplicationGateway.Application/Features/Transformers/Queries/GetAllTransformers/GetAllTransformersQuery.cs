using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Transformers.Queries.GetAllTransformer
{
    public class GetAllTransformersQuery : IRequest<Response<IEnumerable<GetAllTransformersDto>>>
    {
    }
}
