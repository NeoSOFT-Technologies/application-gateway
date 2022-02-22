using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery
{
    public class GetPolicyByIdQuery : IRequest<Response<GetPolicyByIdDto>>
    {
        public Guid PolicyId { get; set; }
    }
}
