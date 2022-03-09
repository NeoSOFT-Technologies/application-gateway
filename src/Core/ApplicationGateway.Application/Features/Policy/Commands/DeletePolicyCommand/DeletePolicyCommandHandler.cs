using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
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
        private readonly ILogger<DeletePolicyCommandHandler> _logger;
        private readonly IPolicyDtoRepository _policyDtoRepository;

        public DeletePolicyCommandHandler(IPolicyDtoRepository policyDtoRepository, IPolicyService policyService, IMapper mapper, ILogger<DeletePolicyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _policyDtoRepository = policyDtoRepository; 
            _policyService = policyService;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Unit> Handle(DeletePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeletePolicyCommand}", request);
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
            await _policyDtoRepository.DeleteAsync(new PolicyDto() { Id=request.PolicyId });
            #endregion

            _logger.LogInformation("Handler Completed");
            return Unit.Value;
        }
    }
}