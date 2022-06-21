using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Common;
using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQuery : SortData,IRequest<PagedResponse<GetAllPoliciesDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
