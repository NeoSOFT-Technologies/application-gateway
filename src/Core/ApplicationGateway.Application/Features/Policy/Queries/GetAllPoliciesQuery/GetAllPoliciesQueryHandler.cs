﻿using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQueryHandler : IRequestHandler<GetAllPoliciesQuery, PagedResponse<GetAllPoliciesDto>>
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPoliciesQueryHandler> _logger;
        private readonly IPolicyService _policyService;
        private readonly IApiService _apiService;

        public GetAllPoliciesQueryHandler(IPolicyRepository policyDtoRepository, IMapper mapper, ILogger<GetAllPoliciesQueryHandler> logger, IPolicyService policyService, IApiService apiService)
        {
            _policyRepository = policyDtoRepository;
            _mapper = mapper;
            _logger = logger;
            _policyService = policyService;
            _apiService = apiService;
        }

        public async Task<PagedResponse<GetAllPoliciesDto>> Handle(GetAllPoliciesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            IReadOnlyList<Domain.Entities.Policy> policyList = await _policyRepository.GetPagedReponseAsync(request.pageNum, request.pageSize);
            int totCount =await _policyRepository.GetTotalCount();
            GetAllPoliciesDto policyDtoList = new GetAllPoliciesDto()
            {
                Policies = _mapper.Map<List<GetAllPolicyModel>>(policyList),
            };

            PagedResponse<GetAllPoliciesDto> response = new PagedResponse<GetAllPoliciesDto>(policyDtoList, totCount, request.pageNum, request.pageSize);
            
            #region Get AuthType of Policy
            foreach (GetAllPolicyModel policy in policyDtoList.Policies)
            {
                List<string> authTypes = new();
                var policyItem = await _policyService.GetPolicyByIdAsync(policy.Id);
                foreach (var api in policyItem.APIs)
                {
                    var apiItem = await _apiService.GetApiByIdAsync(api.Id);
                    if (!authTypes.Contains(apiItem.AuthType))
                    {
                        authTypes.Add(apiItem.AuthType);
                    }
                }
                policy.AuthType = string.Join(", ", authTypes);
            }
            #endregion

            _logger.LogInformation("Handler Completed: {@Response<GetAllPoliciesDto>}", response);
            return response;
        }
    }
}
