using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationGateway.Domain.GatewayCommon;

namespace ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand
{
    public class DeleteApiCommandHandler : IRequestHandler<DeleteApiCommand>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IKeyService _keyService;
        private readonly IPolicyService _policyService;
        private readonly ILogger<DeleteApiCommandHandler> _logger;
        private readonly IApiRepository _apiRepository;
        private readonly IKeyRepository _keyRepository;
        private readonly IPolicyRepository _policyRepository;

        public DeleteApiCommandHandler(IApiRepository apiDtoRepository,IKeyRepository keyDtoRepository, IPolicyRepository policyDtoRepository, ISnapshotService snapshotService, IApiService apiService, IPolicyService policyService, IKeyService keyService, ILogger<DeleteApiCommandHandler> logger)
        {
            _apiRepository = apiDtoRepository;
            _keyRepository = keyDtoRepository;
            _policyRepository = policyDtoRepository;
            _snapshotService = snapshotService;
            _apiService = apiService;
            _keyService = keyService;
            _policyService = policyService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeleteApiCommand}", request);
            Guid apiId = request.ApiId;

            #region Check if API exists
            await _apiService.GetApiByIdAsync(request.ApiId);
            #endregion

            await _apiService.DeleteApiAsync(apiId);

            #region Delete From ApiDto
            await _apiRepository.DeleteAsync(new Domain.Entities.Api() { Id = request.ApiId });
            #endregion

            #region Cascade delete of Policy
            List<Domain.GatewayCommon.Policy> allPolicies = await _policyService.GetAllPoliciesAsync();
            foreach(Domain.GatewayCommon.Policy policy in allPolicies)
            {
                Domain.GatewayCommon.PolicyApi polApi = policy.APIs.Where(polApi => polApi.Id == apiId).FirstOrDefault();
                if(polApi != null)
                {
                    policy.APIs.Remove(polApi);
                    await _policyService.UpdatePolicyAsync(policy);
                    _logger.LogInformation($"Cascade delete of api in policy, policyId: {policy.PolicyId}");

                    #region Update Policy Entity
                    List<string> policyNames = new List<string>();
                    policy.APIs.ForEach(policy => policyNames.Add(policy.Name));
                    Domain.Entities.Policy policyEntity = new Domain.Entities.Policy()
                    {
                        Id = policy.PolicyId,
                        Name = policy.Name,
                        AuthType = "",
                        State = policy.State,
                        Apis = policyNames
                    };
                    await _policyRepository.UpdateAsync(policyEntity);
                    #endregion
                }
            }
            #endregion

            #region Cascade delete of Key
            List<string> allKeysId = await _keyService.GetAllKeysAsync();
            foreach (string keyId in allKeysId)
            {
                Domain.GatewayCommon.Key key = await _keyService.GetKeyAsync(keyId);
                AccessRightsModel keyApi = key.AccessRights.Where(keyApi => keyApi.ApiId == apiId).FirstOrDefault();
                if(keyApi != null)
                {
                    if(key.AccessRights.Count == 1)
                    {
                        await _keyService.DeleteKeyAsync(keyId);
                        #region Delete Key Entity
                        await _keyRepository.DeleteAsync(new Domain.Entities.Key() { Id = keyId });
                        #endregion

                        _logger.LogInformation($"Deletion of key: {keyId}, because of cascade delete");
                    }
                    else
                    {
                        key.AccessRights.Remove(keyApi);
                        await _keyService.UpdateKeyAsync(key);
                        _logger.LogInformation($"Cascade delete of api(access_right) in key, keyId: {key.KeyId}");

                        #region Update Key Entity
                        Domain.Entities.Key keyEntity = new Domain.Entities.Key()
                        {
                            Id = key.KeyId,
                            KeyName = key.KeyName,
                            IsActive = !key.IsInActive,
                            Policies = key.Policies,
                            Expires = key.Expires == 0 ? null : (global::System.DateTimeOffset.FromUnixTimeSeconds(key.Expires)).UtcDateTime
                        };
                        await _keyRepository.UpdateAsync(keyEntity);
                        #endregion
                    }
                }
            }
            #endregion
            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Deleted,
                request.ApiId.ToString(),
                null);
            #endregion

            _logger.LogInformation("Handler Completed: {@Guid}", apiId);
            return Unit.Value;
        }
    }
}