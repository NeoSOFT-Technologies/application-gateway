using ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationGateway.Application.Features.Policy.Commands.UpdatePolicyCommand
{
    public class UpdatePolicyCommandHandler : IRequestHandler<UpdatePolicyCommand, Response<UpdatePolicyDto>>
    {
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePolicyCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public UpdatePolicyCommandHandler(IPolicyService policyService, IMapper mapper, ILogger<UpdatePolicyCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
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

        public async Task<Response<UpdatePolicyDto>> Handle(UpdatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdatePolicyCommand}", request);
            Domain.TykData.Policy policy = _mapper.Map<Domain.TykData.Policy>(request);
            Domain.TykData.Policy newPolicy = await _policyService.UpdatePolicyAsync(policy);

            //HotReload
            await _restClient.GetAsync(null);

            UpdatePolicyDto updatePolicyDto = _mapper.Map<UpdatePolicyDto>(newPolicy);
            Response<UpdatePolicyDto> response = new Response<UpdatePolicyDto>(updatePolicyDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
