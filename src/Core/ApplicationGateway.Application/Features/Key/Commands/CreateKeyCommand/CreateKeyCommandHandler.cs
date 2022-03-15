using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Key.Commands.CreateKeyCommand
{
    public class CreateKeyCommandHandler:IRequestHandler<CreateKeyCommand,Response<Domain.GatewayCommon.Key>>
    {
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly IMapper _mapper;
        readonly ILogger<CreateKeyCommandHandler> _logger;
        readonly IKeyRepository _keyRepository;

        public CreateKeyCommandHandler(IKeyRepository keyDtoRepository, IKeyService keyService, IMapper mapper, ILogger<CreateKeyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _keyRepository = keyDtoRepository;
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Response<Domain.GatewayCommon.Key>> Handle(CreateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateKeyCommandHandler initiated with {request}",request);
            var keyObj = _mapper.Map<Domain.GatewayCommon.Key>(request);
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
            Domain.Entities.Key keyDto = new Domain.Entities.Key() 
            { 
                Id = key.KeyId,
                KeyName = request.KeyName,
                IsActive = !key.IsInActive,
                Policies = key.Policies,
                Expires = key.Expires == 0 ? null : (DateTimeOffset.FromUnixTimeSeconds(key.Expires)).UtcDateTime
        };
            await _keyRepository.AddAsync(keyDto);
            #endregion

            Response<Domain.GatewayCommon.Key> response =new Response<Domain.GatewayCommon.Key>(key, "success");
            _logger.LogInformation("CreateKeyCommandHandler completed with {request}",request);
            return response;
        }
    }
}
