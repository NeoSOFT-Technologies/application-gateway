using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQueryHandler : IRequestHandler<GetKeyQuery, Response<Domain.Entities.Key>>
    {
        readonly ILogger<GetKeyQueryHandler> _logger;
        readonly IMapper _mapper;
        readonly IKeyService _keyService;

        public GetKeyQueryHandler(ILogger<GetKeyQueryHandler> logger, IMapper mapper, IKeyService keyService)
            
        {
            _logger = logger;
            _mapper = mapper;
            _keyService = keyService;
        }

        public async Task<Response<Domain.Entities.Key>> Handle(GetKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetKeyQueryHandler initiated for {request}");
            Domain.Entities.Key key = await _keyService.GetKeyAsync(request.keyId);
            Response<Domain.Entities.Key> response = new Response<Domain.Entities.Key> {Succeeded=true, Data = key, Message = "Success" };
            return response;
        }
    }
}
