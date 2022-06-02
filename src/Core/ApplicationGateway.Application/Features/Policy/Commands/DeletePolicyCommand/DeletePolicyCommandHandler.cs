using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand
{
    public class DeletePolicyCommandHandler : IRequestHandler<DeletePolicyCommand>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IPolicyService _policyService;
        private readonly IKeyService _keyService;
        private readonly ILogger<DeletePolicyCommandHandler> _logger;
        private readonly IPolicyRepository _policyRepository;
        private readonly IKeyRepository _keyRepository;

        public DeletePolicyCommandHandler(IPolicyRepository policyDtoRepository, IPolicyService policyService, IKeyRepository keyDtoRepository, IKeyService keyService, IMapper mapper, ILogger<DeletePolicyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _policyRepository = policyDtoRepository; 
            _policyService = policyService;
            _keyRepository = keyDtoRepository;
            _keyService = keyService;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Unit> Handle(DeletePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeletePolicyCommand}", request);
            Domain.GatewayCommon.Policy policy = await _policyService.GetPolicyByIdAsync(request.PolicyId);
            await _policyService.DeletePolicyAsync(request.PolicyId);

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Policy,
                Enums.Operation.Deleted,
                request.PolicyId.ToString(),
                null);
            #endregion

            #region Delete Policy Dto
            await _policyRepository.DeleteAsync(new Domain.Entities.Policy() { Id= request.PolicyId });
            #endregion


            #region Cascading delete of Key
            List<string> allKeysId = await _keyService.GetAllKeysAsync();
            foreach (string keyId in allKeysId)
            {
                Domain.GatewayCommon.Key key = await _keyService.GetKeyAsync(keyId);
                // Iterate policies in key
                if (key.Policies != null)
                {
                    foreach (var policyId in key.Policies)
                    {
                        // Check deleting policy exists in key
                        if (Guid.Parse(policyId) == policy.PolicyId)
                        {
                            if (key.Policies.Count == 1)
                            {
                                await _keyService.DeleteKeyAsync(keyId);
                                #region Delete Key Dto
                                await _keyRepository.DeleteAsync(new Domain.Entities.Key() { Id = keyId });
                                #endregion
                                _logger.LogInformation("Delete key because of cascading delete for policy");
                            }
                            else
                            {
                                // Remove all APIs in key which were referenced in deleting policy
                                foreach (Domain.GatewayCommon.PolicyApi api in policy.APIs)
                                {
                                    key.AccessRights.RemoveAll(x => x.ApiId == api.Id);
                                }
                                key.Policies.Remove(policyId);
                                await _keyService.UpdateKeyAsync(key);

                                _logger.LogInformation($"Cascade delete of api(access_right) in key, keyId: {key.KeyId}");

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
                            }
                            break;
                        }
                    }
                }
            }
                #endregion
                _logger.LogInformation("Handler Completed: {@Guid}", request.PolicyId);
            return Unit.Value;
        }
    }    
}