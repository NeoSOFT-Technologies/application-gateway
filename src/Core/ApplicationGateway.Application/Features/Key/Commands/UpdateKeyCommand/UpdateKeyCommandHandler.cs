using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand
{
    public class UpdateKeyCommandHandler:IRequestHandler<UpdateKeyCommand,Response<UpdateKeyCommandDto>>
    {
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<UpdateKeyCommandHandler> _logger;

        public UpdateKeyCommandHandler(IKeyService keyService, IMapper mapper, ILogger<UpdateKeyCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration, ISnapshotService snapshotService)
        {
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<UpdateKeyCommandDto>> Handle(UpdateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateKeyHandler initiated for {request}");

            #region Check if Key exists
            await _keyService.GetKeyAsync(request.KeyId);
            #endregion

            Domain.Entities.Key key = await _keyService.UpdateKeyAsync(_mapper.Map<Domain.Entities.Key>(request));

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Key,
                Enums.Operation.Updated,
                request.KeyId.ToString(),
                key);

            UpdateKeyCommandDto updateKeyCommandDto = _mapper.Map<UpdateKeyCommandDto>(key);
            Response<UpdateKeyCommandDto> response = new Response<UpdateKeyCommandDto>() {Succeeded=true,Data=updateKeyCommandDto,Message="success" };
            _logger.LogInformation($"UpdateKeyHandler completed for {request}");
            return response;
        }
    }
}
