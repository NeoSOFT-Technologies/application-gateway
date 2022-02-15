using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Application.Responses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Application.Features.Api.Commands.UpdateApiCommand
{
    public class UpdateApiCommandHandler : IRequestHandler<UpdateApiCommand, Response<UpdateApiDto>>
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateApiCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public UpdateApiCommandHandler(IApiService apiService, IMapper mapper, ILogger<UpdateApiCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
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

        public async Task<Response<UpdateApiDto>> Handle(UpdateApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@UpdateApiCommand}", request);
            Guid apiId = request.ApiId;

            #region Check if API exists
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            RestClient<string> restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/apis", headers);
            string response = await restClient.GetAsync(apiId.ToString());
            JObject responseObject = JObject.Parse(response);
            if (responseObject["message"] is not null && responseObject["message"].ToString() == "API not found")
                throw new NotFoundException("API", request.ApiId);
            #endregion

            Domain.TykData.Api api = _mapper.Map<Domain.TykData.Api>(request);
            Domain.TykData.Api newApi = await _apiService.UpdateApiAsync(api);

            await _restClient.GetAsync(null);

            UpdateApiDto updateApiDto = _mapper.Map<UpdateApiDto>(newApi);
            Response<UpdateApiDto> result = new Response<UpdateApiDto>(updateApiDto, "success");

            _logger.LogInformation("Handler Completed");
            return result;
        }
    }
}
