using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQueryHandler : IRequestHandler<GetAllPoliciesQuery, Response<List<GetAllPoliciesDto>>>
    {
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPoliciesQueryHandler> _logger;

        public GetAllPoliciesQueryHandler(IPolicyService policyService, IMapper mapper, ILogger<GetAllPoliciesQueryHandler> logger)
        {
            _policyService = policyService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<List<GetAllPoliciesDto>>> Handle(GetAllPoliciesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            List<Domain.Entities.Policy> policyList = await _policyService.GetAllPoliciesAsync();
            //List<GetAllPoliciesDto> getAllApisDto = _mapper.Map<List<GetAllPoliciesDto>>(policyList);
            //var response = new Response<List<GetAllPoliciesDto>>(getAllApisDto, "success");

            //policyList.ForEach(x=>x.)
            List<GetAllPoliciesDto> policyDtoList = new List<GetAllPoliciesDto>();
            foreach (var policy in policyList)
            {
                GetAllPoliciesDto policyDto = new GetAllPoliciesDto();
                policyDto.PolicyName = policy.Name;

                #region Set Policy Status
                if (policy.Active)
                    policyDto.Status = "Active";
                else
                    policyDto.Status = "InActive";
                #endregion

                #region Set List of Api Name
                List<string> ApiList = new List<string>();
                policy.APIs.ForEach(x => ApiList.Add(x.Name));
                #endregion

                policyDto.AccessRights = ApiList;
                policyDto.AuthType = "Auth Token";

   
                policyDtoList.Add(policyDto);
            }

            var response = new Response<List<GetAllPoliciesDto>>(policyDtoList, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
