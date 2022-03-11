using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetAllKeys
{
    public class GetAllKeysQuery:IRequest<Response<GetAllKeysDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
