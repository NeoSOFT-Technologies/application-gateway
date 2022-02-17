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

namespace ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand
{
    public class CreateKeyCommandHandler:IRequestHandler<CreateKeyCommand,Response<Domain.Entities.Key>>
    {
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<CreateKeyCommandHandler> _logger;

        public CreateKeyCommandHandler(IKeyService keyService, IMapper mapper, ILogger<CreateKeyCommandHandler> logger)
        {
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<Domain.Entities.Key>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateKeyCommandHandler initiated with {request}");
            var keyObj = _mapper.Map<Domain.Entities.Key>(request);
            var key = await _keyService.CreateKeyAsync(keyObj);

            Response<Domain.Entities.Key> response =new Response<Domain.Entities.Key>(key, "success");
            return response;
        }
    }
}
