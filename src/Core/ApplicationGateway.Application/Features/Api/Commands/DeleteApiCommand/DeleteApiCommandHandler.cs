using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Application.Features.Api.Commands.DeleteApiCommand
{
    public class DeleteApiCommandHandler : IRequestHandler<DeleteApiCommand>
    {
        private readonly ISnapshotService _snapshotService;
        private readonly IApiService _apiService;
        private readonly ILogger<DeleteApiCommandHandler> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public DeleteApiCommandHandler(ISnapshotService snapshotService, IApiService apiService, ILogger<DeleteApiCommandHandler> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _snapshotService = snapshotService;
            _apiService = apiService;
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
        }

        public async Task<Unit> Handle(DeleteApiCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handler Initiated with {@DeleteApiCommand}", request);
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

            await _apiService.DeleteApiAsync(apiId);

            await _restClient.GetAsync(null);

            await _snapshotService.CreateSnapshot(
                Enums.Gateway.Tyk,
                Enums.Type.API,
                Enums.Operation.Deleted,
                apiId,
                null);

            _logger.LogInformation("Handler Completed");
            return Unit.Value;
        }
    }
}
