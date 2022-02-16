using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationGateway.Application.Features.Api.Commands.CreateApiCommand
{
    public class CreateApiCommandHandler : IRequestHandler<CreateApiCommand, Response<CreateApiDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateApiCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public CreateApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, IMapper mapper, ILogger<CreateApiCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _snapshotService = snapshotService;
            _apiService = apiService;
            _mapper = mapper;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
        }

        public async Task<Response<CreateApiDto>> Handle(CreateApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateApiCommand}", request);
            Domain.TykData.Api api = _mapper.Map<Domain.TykData.Api>(request);
            Domain.TykData.Api newApi = await _apiService.CreateApiAsync(api);

            await _restClient.GetAsync(null);

            CreateApiDto createApiDto = _mapper.Map<CreateApiDto>(newApi);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Created,
                createApiDto.ApiId,
                newApi);

            Response<CreateApiDto> response = new Response<CreateApiDto>(createApiDto, "success");

            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
