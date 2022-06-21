using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetAllKeys
{
    public class GetAllKeysQuery:SortData,IRequest<PagedResponse<GetAllKeysDto>>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }

    }
}
