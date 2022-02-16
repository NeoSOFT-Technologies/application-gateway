using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQuery : IRequest<Response<GetAllApisDto>>
    {
    }
}
