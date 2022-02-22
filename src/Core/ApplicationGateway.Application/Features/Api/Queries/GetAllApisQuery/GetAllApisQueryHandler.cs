using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Queries.GetAllApisQuery
{
    public class GetAllApisQueryHandler : IRequestHandler<GetAllApisQuery, Response<GetAllApisDto>>
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllApisQueryHandler> _logger;

        public GetAllApisQueryHandler(IApiService apiService, IMapper mapper, ILogger<GetAllApisQueryHandler> logger)
        {
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetAllApisDto>> Handle(GetAllApisQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated");
            List<Domain.Entities.Api> apiList = await _apiService.GetAllApisAsync();
            GetAllApisDto getAllApisDto = new GetAllApisDto() { Apis = new List<GetAllApiModel>() };

            foreach (var api in apiList)
            {
                getAllApisDto.Apis.Add(_mapper.Map<GetAllApiModel>(api));
            }

            var response = new Response<GetAllApisDto>(getAllApisDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}