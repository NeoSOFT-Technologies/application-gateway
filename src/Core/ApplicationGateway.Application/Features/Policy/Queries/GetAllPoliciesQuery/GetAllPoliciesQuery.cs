using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Common;
using MediatR;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQuery : SortSearchData,IRequest<PagedResponse<GetAllPoliciesDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
