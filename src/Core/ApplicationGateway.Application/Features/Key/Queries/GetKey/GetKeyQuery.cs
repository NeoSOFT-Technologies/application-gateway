using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQuery:IRequest<Response<Domain.TykData.Key>>
    {
        public string keyId { get; set; }
    }
}
