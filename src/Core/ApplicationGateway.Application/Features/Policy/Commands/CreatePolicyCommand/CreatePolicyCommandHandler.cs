using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand
{
    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Response<CreatePolicyDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePolicyCommandHandler> _logger;
        private readonly IPolicyRepository _policyRepository;

        public CreatePolicyCommandHandler(IPolicyRepository policyDtoRepository, ISnapshotService snapshotService, IPolicyService policyService, IMapper mapper, ILogger<CreatePolicyCommandHandler> logger)
        {
            _policyRepository= policyDtoRepository;
            _snapshotService = snapshotService;
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<CreatePolicyDto>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreatePolicyCommand}", request);
            Domain.GatewayCommon.Policy policy = _mapper.Map<Domain.GatewayCommon.Policy>(request);
            Domain.GatewayCommon.Policy newPolicy = await _policyService.CreatePolicyAsync(policy);

            CreatePolicyDto createPolicyDto = _mapper.Map<CreatePolicyDto>(newPolicy);

            #region Create Snapshot
            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Policy,
                Enums.Operation.Created,
                newPolicy.PolicyId.ToString(),
                newPolicy);
            #endregion

            #region Create Policy Dto
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
            await _policyRepository.AddAsync(policyDto);
            #endregion

            Response<CreatePolicyDto> response = new Response<CreatePolicyDto>(createPolicyDto, "success");
            _logger.LogInformation("Handler Completed: {@Response<CreatePolicyDto>}", response);
            return response;
        }
    }
}