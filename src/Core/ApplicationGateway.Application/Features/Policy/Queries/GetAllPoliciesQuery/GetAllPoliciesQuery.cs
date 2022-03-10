using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQuery : IRequest<Response<GetAllPoliciesDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
