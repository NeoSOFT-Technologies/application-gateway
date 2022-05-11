using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Features.Policy.Queries.GetPolicyByIdQuery;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using static ApplicationGateway.Application.Features.Key.Queries.GetKey.GetKeyDto;

namespace ApplicationGateway.Application.Features.Key.Queries.GetKey
{
    public class GetKeyQueryHandler : IRequestHandler<GetKeyQuery, Response<GetKeyDto>>
    {
        readonly ILogger<GetKeyQueryHandler> _logger;
        readonly IKeyService _keyService;
        readonly IMapper _mapper;   
        readonly IApiService _apiService;
        readonly IPolicyService _policyService;

        public GetKeyQueryHandler(ILogger<GetKeyQueryHandler> logger, IKeyService keyService, IMapper mapper, IApiService apiService, IPolicyService policyService)
            
        {
            _logger = logger;
            _keyService = keyService;
            _mapper = mapper;
            _apiService = apiService;
            _policyService = policyService;
        }

        public async Task<Response<GetKeyDto>> Handle(GetKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetKeyQueryHandler initiated for {request}", request);
            Domain.GatewayCommon.Key key = await _keyService.GetKeyAsync(request.keyId);
            GetKeyDto getKeyDto = _mapper.Map<GetKeyDto>(key);
            #region Add MasterVersions in AccessRights for Dto
            foreach (var api in getKeyDto.AccessRights)
            {
                List<string> allApiVersions = new();
                Domain.GatewayCommon.Api apiObj = await _apiService.GetApiByIdAsync(api.ApiId);
                apiObj.Versions.ForEach(v => allApiVersions.Add(v.Name)); 
                api.MasterVersions = allApiVersions;
                api.AuthType = apiObj.AuthType;
            }
            #endregion

            #region Add Policy Data in PolicyById, for key for policies
            if (getKeyDto.Policies.Any())
            {
                List<PolicyById> policyByIdsList = new();
                foreach (var policy in getKeyDto.Policies)
                {
                    Domain.GatewayCommon.Policy policyObj = await _policyService.GetPolicyByIdAsync(Guid.Parse(policy));
                    PolicyById policyById = _mapper.Map<PolicyById>(policyObj);
                    #region Set isDisabled for RateLimit and Quota
                    foreach (Domain.GatewayCommon.PolicyApi api in policyById.APIs)
                    {
                        Domain.GatewayCommon.Api apiObj = await _apiService.GetApiByIdAsync(api.Id);
                        api.isRateLimitDisabled = apiObj.RateLimit.IsDisabled;
                        api.isQuotaDisabled = apiObj.IsQuotaDisabled;
                    }
                    #endregion
                    policyById.Global = _mapper.Map<GlobalPolicy>(policyObj);
                    policyByIdsList.Add(policyById);
                }
                getKeyDto.PolicyByIds = policyByIdsList;
            }
            #endregion
            Response<GetKeyDto> response = new Response<GetKeyDto> {Succeeded=true, Data = getKeyDto, Message = "Success" };
            _logger.LogInformation("GetKeyQueryHandler completed for {request}", request);
            return response;
        }
    }
}
