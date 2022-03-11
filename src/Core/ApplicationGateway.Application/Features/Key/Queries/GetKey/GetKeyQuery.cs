using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQuery:IRequest<Response<Domain.GatewayCommon.Key>>
    {
        public string keyId { get; set; }
    }
}
