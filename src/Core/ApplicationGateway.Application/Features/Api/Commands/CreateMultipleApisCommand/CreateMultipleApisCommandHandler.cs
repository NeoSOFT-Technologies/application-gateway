using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand
{
    public class CreateMultipleApisCommandHandler : IRequestHandler<CreateMultipleApisCommand, Response<CreateMultipleApisDto>>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMultipleApisCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public CreateMultipleApisCommandHandler(ISnapshotService snapshotService, IApiService apiService, IMapper mapper, ILogger<CreateMultipleApisCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
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
        public async Task<Response<CreateMultipleApisDto>> Handle(CreateMultipleApisCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@CreateMultipleApisCommand}", request);
            #region Check for repeated listen path in request
            if (request.APIs.DistinctBy(p => p.ListenPath.Trim('/')).Count() != request.APIs.Count())
            {
                throw new BadRequestException("Listen path should be unique");
            }
            #endregion

            #region Get All Existsing APIs
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            RestClient<string> restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/apis", headers);
            JArray allApis = JArray.Parse(await restClient.GetAsync(null));
            #endregion

            #region Add APIs one by one
            CreateMultipleApisDto createMultipleApisDto = new CreateMultipleApisDto() { APIs = new List<MultipleApiModelDto>() };
            foreach (MultipleApiModel obj in request.APIs)
            {
                #region Check for existing listenPath
                foreach (JToken API in allApis)
                {
                    string listen_path = API["proxy"]["listen_path"].ToString();
                    if (obj.ListenPath.Trim('/') == listen_path.Trim('/'))
                    {
                        throw new BadRequestException("Listen path already exists");
                    }
                }
                #endregion

                Domain.TykData.Api api = _mapper.Map<Domain.TykData.Api>(obj);
                Domain.TykData.Api newApi = await _apiService.CreateApiAsync(api);

                MultipleApiModelDto multipleApiModelDto = _mapper.Map<MultipleApiModelDto>(newApi);
                createMultipleApisDto.APIs.Add(multipleApiModelDto);

                await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Created,
                newApi.ApiId,
                newApi);
            }
            #endregion

            await _restClient.GetAsync(null);
            Response<CreateMultipleApisDto> response = new Response<CreateMultipleApisDto>(createMultipleApisDto, "success");

            _logger.LogInformation("Handler Completed");
            return response;
        }
    }
}
