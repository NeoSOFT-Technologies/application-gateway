﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQueryHandler : IRequestHandler<GetKeyQuery, Response<Domain.GatewayCommon.Key>>
    {
        readonly ILogger<GetKeyQueryHandler> _logger;
        readonly IKeyService _keyService;

        public GetKeyQueryHandler(ILogger<GetKeyQueryHandler> logger, IKeyService keyService)
            
        {
            _logger = logger;
            _keyService = keyService;
        }

        public async Task<Response<Domain.GatewayCommon.Key>> Handle(GetKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetKeyQueryHandler initiated for {request}", request);
            Domain.GatewayCommon.Key key = await _keyService.GetKeyAsync(request.keyId);
            Response<Domain.GatewayCommon.Key> response = new Response<Domain.GatewayCommon.Key> {Succeeded=true, Data = key, Message = "Success" };
            _logger.LogInformation("GetKeyQueryHandler completed for {request}", request);
            return response;
        }
    }
}
