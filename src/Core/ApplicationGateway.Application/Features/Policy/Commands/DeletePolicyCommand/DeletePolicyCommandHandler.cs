using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Commands.DeletePolicyCommand
{
    public class DeletePolicyCommandHandler : IRequestHandler<DeletePolicyCommand>
    {
        private readonly IPolicyService _policyService;
        private readonly ILogger<DeletePolicyCommandHandler> _logger;

        public DeletePolicyCommandHandler(IPolicyService policyService, IMapper mapper, ILogger<DeletePolicyCommandHandler> logger)
        {
            _policyService = policyService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePolicyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeletePolicyCommand}", request);
            await _policyService.DeletePolicyAsync(request.PolicyId);

            _logger.LogInformation("Handler Completed");
            return Unit.Value;
        }
    }
}