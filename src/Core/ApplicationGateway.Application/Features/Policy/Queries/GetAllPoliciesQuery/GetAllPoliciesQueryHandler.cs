using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQueryHandler : IRequestHandler<GetAllPoliciesQuery, Response<GetAllPoliciesDto>>
    {
        private readonly IPolicyDtoRepository _policyDtoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPoliciesQueryHandler> _logger;

        public GetAllPoliciesQueryHandler(IPolicyDtoRepository policyDtoRepository, IMapper mapper, ILogger<GetAllPoliciesQueryHandler> logger)
        {
            _policyDtoRepository = policyDtoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetAllPoliciesDto>> Handle(GetAllPoliciesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            IReadOnlyList<PolicyDto> policyList = await _policyDtoRepository.ListAllAsync();
            GetAllPoliciesDto policyDtoList = new GetAllPoliciesDto()
            {
                Policies = _mapper.Map<List<GetAllPolicyModel>>(policyList),
            };

            var response = new Response<GetAllPoliciesDto>(policyDtoList, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
