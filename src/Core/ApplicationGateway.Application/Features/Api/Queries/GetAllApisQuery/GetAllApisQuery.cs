using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Common;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQuery : SortData,IRequest<PagedResponse<GetAllApisDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
