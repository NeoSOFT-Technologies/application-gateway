using ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand
{
    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Response<CreatePolicyDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePolicyCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public CreatePolicyCommandHandler(ISnapshotService snapshotService, IPolicyService policyService, IMapper mapper, ILogger<CreatePolicyCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _snapshotService = snapshotService;
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
        }

        public async Task<Response<CreatePolicyDto>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreatePolicyCommand}", request);
            Domain.TykData.Policy policy = _mapper.Map<Domain.TykData.Policy>(request);
            Domain.TykData.Policy newPolicy = await _policyService.CreatePolicyAsync(policy);

            //HotReload
            await _restClient.GetAsync(null);

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
