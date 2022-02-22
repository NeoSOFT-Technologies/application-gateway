using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand
{
    public class UpdatePolicyCommandHandler : IRequestHandler<UpdatePolicyCommand, Response<UpdatePolicyDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePolicyCommandHandler> _logger;

        public UpdatePolicyCommandHandler(IPolicyService policyService, IMapper mapper, ILogger<UpdatePolicyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Response<UpdatePolicyDto>> Handle(UpdatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdatePolicyCommand}", request);
            Domain.Entities.Policy policy = _mapper.Map<Domain.Entities.Policy>(request);
            Domain.Entities.Policy newPolicy = await _policyService.UpdatePolicyAsync(policy);

            UpdatePolicyDto updatePolicyDto = _mapper.Map<UpdatePolicyDto>(newPolicy);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Policy,
                Enums.Operation.Updated,
                request.PolicyId.ToString(),
                newPolicy);

            Response<UpdatePolicyDto> response = new Response<UpdatePolicyDto>(updatePolicyDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}