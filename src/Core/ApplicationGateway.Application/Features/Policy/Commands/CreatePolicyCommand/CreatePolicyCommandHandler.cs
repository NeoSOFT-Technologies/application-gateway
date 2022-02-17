using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Responses;
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

        public CreatePolicyCommandHandler(ISnapshotService snapshotService, IPolicyService policyService, IMapper mapper, ILogger<CreatePolicyCommandHandler> logger)
        {
            _snapshotService = snapshotService;
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<CreatePolicyDto>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreatePolicyCommand}", request);
            Domain.Entities.Policy policy = _mapper.Map<Domain.Entities.Policy>(request);
            Domain.Entities.Policy newPolicy = await _policyService.CreatePolicyAsync(policy);

            CreatePolicyDto createPolicyDto = _mapper.Map<CreatePolicyDto>(newPolicy);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.Policy,
                Enums.Operation.Created,
                createPolicyDto.PolicyId,
                newPolicy);

            Response<CreatePolicyDto> response = new Response<CreatePolicyDto>(createPolicyDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}