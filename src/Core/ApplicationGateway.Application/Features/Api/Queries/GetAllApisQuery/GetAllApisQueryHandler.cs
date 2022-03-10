using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Contracts.Persistence.IDtoRepositories;
using ApplicationGateway.Application.Responses;
using ApplicationGateway.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQueryHandler : IRequestHandler<GetAllApisQuery, Response<GetAllApisDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllApisQueryHandler> _logger;
        private readonly IApiDtoRepository _apiDtoRepository;

        public GetAllApisQueryHandler(IApiDtoRepository apiDtoRepository, IMapper mapper, ILogger<GetAllApisQueryHandler> logger)
        {
            _apiDtoRepository = apiDtoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetAllApisDto>> Handle(GetAllApisQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            IReadOnlyList<ApiDto> apiList = await _apiDtoRepository.ListAllAsync();
            GetAllApisDto getAllApisDto = new GetAllApisDto()
            {
                Apis = _mapper.Map<List<GetAllApiModel>>(apiList)
            };

            var response = new Response<GetAllApisDto>(getAllApisDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}