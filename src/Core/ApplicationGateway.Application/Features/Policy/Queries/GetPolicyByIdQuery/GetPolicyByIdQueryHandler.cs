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
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPolicyByIdQueryHandler> _logger;

        public GetPolicyByIdQueryHandler(IPolicyService policyService, IApiService apiService, IMapper mapper, ILogger<GetPolicyByIdQueryHandler> logger)
        {
            _policyService = policyService;
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetPolicyByIdDto>> Handle(GetPolicyByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@GetPolicyByIdQuery}", request);
            Domain.GatewayCommon.Policy policy = await _policyService.GetPolicyByIdAsync(request.PolicyId);
            GetPolicyByIdDto getPolicyByIdDto = _mapper.Map<GetPolicyByIdDto>(policy);

            #region Add MasterVersions in AccessRights for Dto & Set isDisabled for RateLimit & Quota
            foreach (GetPolicyApi api in getPolicyByIdDto.APIs)
            {
                List<string> masterVersions = new();
                Domain.GatewayCommon.Api apiObj = await _apiService.GetApiByIdAsync(api.Id);
                apiObj.Versions.ForEach(v => masterVersions.Add(v.Name));
                api.MasterVersions = masterVersions;
                api.AuthType = apiObj.AuthType;
                api.isRateLimitDisabled = apiObj.RateLimit.IsDisabled;
                api.isQuotaDisbaled = apiObj.IsQuotaDisabled;
            }
            #endregion

            Response<GetPolicyByIdDto> response = new Response<GetPolicyByIdDto>(getPolicyByIdDto, "success");
            _logger.LogInformation("Handler Completed: {@Response<GetPolicyByIdDto>}", response);
            return response;
        }
    }
}
