using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Policy.Queries.GetAllPoliciesQuery
{
    public class GetAllPoliciesQueryHandler : IRequestHandler<GetAllPoliciesQuery, Response<GetAllPoliciesDto>>
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPoliciesQueryHandler> _logger;

        public GetAllPoliciesQueryHandler(IPolicyRepository policyDtoRepository, IMapper mapper, ILogger<GetAllPoliciesQueryHandler> logger)
        {
            _policyRepository = policyDtoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetAllPoliciesDto>> Handle(GetAllPoliciesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            IReadOnlyList<Domain.Entities.Policy> policyList = await _policyRepository.GetPagedReponseAsync(request.pageNum, request.pageSize);
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
