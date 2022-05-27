using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Key.Commands.UpdateKeyCommand
{
    public class UpdateKeyCommandHandler:IRequestHandler<UpdateKeyCommand,Response<UpdateKeyCommandDto>>
    {
        readonly ISnapshotService _snapshotService;
        readonly IKeyService _keyService;
        readonly IPolicyService _policyService;
        readonly IMapper _mapper;
        readonly ILogger<UpdateKeyCommandHandler> _logger;
        readonly IKeyRepository _keyRepository;

        public UpdateKeyCommandHandler(IKeyRepository keyDtoRepository, IKeyService keyService, IMapper mapper, ILogger<UpdateKeyCommandHandler> logger, ISnapshotService snapshotService, IPolicyService policyService)
        {
            _keyRepository = keyDtoRepository;
            _keyService = keyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
            _policyService = policyService;
        }

        public async Task<Response<UpdateKeyCommandDto>> Handle(UpdateKeyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateKeyHandler initiated for {request}", request);

            #region Validate if policies exists, if entered
            if (request.Policies.Any())
                foreach (var policy in request.Policies)
                    await _policyService.GetPolicyByIdAsync(Guid.Parse(policy));
            #endregion

            #region Check if Key exists
            await _keyService.GetKeyAsync(request.KeyId);
            #endregion

            Domain.GatewayCommon.Key key = await _keyService.UpdateKeyAsync(_mapper.Map<Domain.GatewayCommon.Key>(request));

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Key,
                Enums.Operation.Updated,
                request.KeyId.ToString(),
                key);
            #endregion

            #region Update Key Dto
            Domain.Entities.Key keyDto = new Domain.Entities.Key()
            {
                Id = key.KeyId,
                KeyName = key.KeyName,
                IsActive = !key.IsInActive,
                Policies = key.Policies,
                Expires = key.Expires == 0 ? null : (global::System.DateTimeOffset.FromUnixTimeSeconds(key.Expires)).UtcDateTime
            };
            await _keyRepository.UpdateAsync(keyDto);
            #endregion


            UpdateKeyCommandDto updateKeyCommandDto = _mapper.Map<UpdateKeyCommandDto>(key);
            Response<UpdateKeyCommandDto> response = new Response<UpdateKeyCommandDto>() {Succeeded=true,Data=updateKeyCommandDto,Message="success" };
            _logger.LogInformation("UpdateKeyHandler completed for {request}", request);
            return response;
        }
    }
}
