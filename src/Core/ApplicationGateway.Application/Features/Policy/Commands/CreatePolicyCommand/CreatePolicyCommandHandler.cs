using ApplicationGateway.Application.Contracts.Infrastructure.Tyk;
using ApplicationGateway.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApplicationGateway.Application.Features.Policy.Commands.CreatePolicyCommand
{
    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Response<CreatePolicyDto>>
    {
        private readonly IPolicyService _policyService;
        private readonly ILogger<CreatePolicyCommandHandler> _logger;

        public CreatePolicyCommandHandler(IPolicyService policyService, ILogger<CreatePolicyCommandHandler> logger)
        {
            _policyService = policyService;
            _logger = logger;
        }

        public async Task<Response<CreatePolicyDto>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            string requestJson = JsonConvert.SerializeObject(request);

            string policyId = await _policyService.CreatePolicy(requestJson);

            CreatePolicyDto createPolicyDto = new CreatePolicyDto()
            {
                PolicyId = Guid.Parse(policyId),
                Name = request.Name
            };

            Response<CreatePolicyDto> response = new Response<CreatePolicyDto>(createPolicyDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
