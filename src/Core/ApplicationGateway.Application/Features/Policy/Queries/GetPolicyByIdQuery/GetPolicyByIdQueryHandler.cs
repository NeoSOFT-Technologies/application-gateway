using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery
{
    public class GetPolicyByIdQueryHandler : IRequestHandler<GetPolicyByIdQuery, Response<GetPolicyByIdDto>>
    {
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPolicyByIdQueryHandler> _logger;

        public GetPolicyByIdQueryHandler(IPolicyService policyService, IMapper mapper, ILogger<GetPolicyByIdQueryHandler> logger)
        {
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetPolicyByIdDto>> Handle(GetPolicyByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@GetPolicyByIdQuery}", request);
            Domain.GatewayCommon.Policy policy = await _policyService.GetPolicyByIdAsync(request.PolicyId);
            GetPolicyByIdDto getPolicyByIdDto = _mapper.Map<GetPolicyByIdDto>(policy);
            var response = new Response<GetPolicyByIdDto>(getPolicyByIdDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
