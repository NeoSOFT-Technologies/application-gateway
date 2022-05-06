using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
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
        private readonly IPolicyRepository _policyRepository;

        public UpdatePolicyCommandHandler(IPolicyRepository policyDtoRepository, IPolicyService policyService, IMapper mapper, ILogger<UpdatePolicyCommandHandler> logger, ISnapshotService snapshotService)
        {
            _policyRepository = policyDtoRepository;
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
            _snapshotService = snapshotService;
        }

        public async Task<Response<UpdatePolicyDto>> Handle(UpdatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdatePolicyCommand}", request);
            Domain.GatewayCommon.Policy policy = _mapper.Map<Domain.GatewayCommon.Policy>(request);
            Domain.GatewayCommon.Policy newPolicy = await _policyService.UpdatePolicyAsync(policy);

            UpdatePolicyDto updatePolicyDto = _mapper.Map<UpdatePolicyDto>(newPolicy);

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Policy,
                Enums.Operation.Updated,
                request.PolicyId.ToString(),
                newPolicy);
            #endregion

            #region Update Policy Dto
            List<string> policyNames = new List<string>();
            newPolicy.APIs.ForEach(policy => policyNames.Add(policy.Name));

            Domain.Entities.Policy policyDto = new Domain.Entities.Policy()
            {
                Id = newPolicy.PolicyId,
                Name = newPolicy.Name,
                AuthType = "",
                State = newPolicy.State,
                Apis = policyNames
            };
            await _policyRepository.UpdateAsync(policyDto);
            #endregion

            Response<UpdatePolicyDto> response = new Response<UpdatePolicyDto>(updatePolicyDto, "success");
            _logger.LogInformation("Handler Completed: {@Response<CreatePolicyDto>}", response);
            return response;
        }
    }
}