using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<CreateKeyCommandHandler> _logger;
        readonly IKeyDtoRepository _keyDtoRepository;

        public CreateKeyCommandHandler(IKeyDtoRepository keyDtoRepository, IKeyService keyService, IMapper mapper, ILogger<CreateKeyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _keyDtoRepository = keyDtoRepository;
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Response<Domain.Entities.Key>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateKeyCommandHandler initiated with {request}");
            var keyObj = _mapper.Map<Domain.Entities.Key>(request);
            var key = await _keyService.CreateKeyAsync(keyObj);

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Key,
                Enums.Operation.Created,
                key.KeyId,
                key);
            #endregion

            #region Create Key Dto
            KeyDto keyDto = new KeyDto() 
            { 
                Id = key.KeyId,
                KeyName = request.KeyName,
                IsActive = !key.IsInActive,
                Policies = key.Policies,
                Expires = key.Expires == 0 ? null : (DateTimeOffset.FromUnixTimeSeconds(key.Expires)).LocalDateTime
        };
            await _keyDtoRepository.AddAsync(keyDto);
            #endregion

            Response<Domain.Entities.Key> response =new Response<Domain.Entities.Key>(key, "success");
            return response;
        }
    }
}
