using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationGateway.Application.Features.Api.Queries.GetApiByIdQuery
{
    public class GetApiByIdQueryHandler : IRequestHandler<GetApiByIdQuery, Response<GetApiByIdDto>>
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetApiByIdQueryHandler> _logger;

        public GetApiByIdQueryHandler(IApiService apiService, IMapper mapper, ILogger<GetApiByIdQueryHandler> logger)
        {
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GetApiByIdDto>> Handle(GetApiByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@GetApiByIdQuery}", request);
            Domain.GatewayCommon.Api api = await _apiService.GetApiByIdAsync(request.ApiId);
            GetApiByIdDto getApiByIdDto = _mapper.Map<GetApiByIdDto>(api);
            var response = new Response<GetApiByIdDto>(getApiByIdDto, "success");
            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}