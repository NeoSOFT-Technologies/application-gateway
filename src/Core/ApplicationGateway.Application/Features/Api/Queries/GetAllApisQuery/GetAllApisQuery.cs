using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQuery : IRequest<Response<GetAllApisDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
