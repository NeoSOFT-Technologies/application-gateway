﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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
        readonly IKeyDtoRepository _keyDtoRepository;

        public UpdateKeyCommandHandler(IKeyDtoRepository keyDtoRepository, IKeyService keyService, IMapper mapper, ILogger<UpdateKeyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _keyDtoRepository = keyDtoRepository;
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Response<UpdateKeyCommandDto>> Handle(UpdateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateKeyHandler initiated for {request}");

            #region Check if Key exists
            await _keyService.GetKeyAsync(request.KeyId);
            #endregion

            Domain.Entities.Key key = await _keyService.UpdateKeyAsync(_mapper.Map<Domain.Entities.Key>(request));

            #region Create SnapShot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Key,
                Enums.Operation.Updated,
                request.KeyId.ToString(),
                key);
            #endregion

            #region Update Key Dto
            KeyDto keyDto = new KeyDto()
            {
                Id = key.KeyId,
                KeyName = request.KeyName,
                IsActive = !key.IsInActive,
                Policies = key.Policies,
                Expires = key.Expires == 0 ? null : (DateTimeOffset.FromUnixTimeSeconds(key.Expires)).LocalDateTime
            };
            await _keyDtoRepository.UpdateAsync(keyDto);
            #endregion


            UpdateKeyCommandDto updateKeyCommandDto = _mapper.Map<UpdateKeyCommandDto>(key);
            Response<UpdateKeyCommandDto> response = new Response<UpdateKeyCommandDto>() {Succeeded=true,Data=updateKeyCommandDto,Message="success" };
            _logger.LogInformation($"UpdateKeyHandler completed for {request}");
            return response;
        }
    }
}
