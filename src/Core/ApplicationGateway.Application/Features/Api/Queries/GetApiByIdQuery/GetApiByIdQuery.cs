using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery
{
    public class GetApiByIdQuery : IRequest<Response<GetApiByIdDto>>
    {
        public Guid ApiId { get; set; }
    }
}
