using ApplicationGateway.Application.Contracts.Infrastructure.Gateway.Tyk;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationGateway.Infrastructure.Gateway.Tyk
{
    public class TykBaseService : IBaseService
    {
        private readonly ILogger<TykBaseService> _logger;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;
        public TykBaseService(ILogger<TykBaseService> logger, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
        }

        public async Task HotReload()
        {
            _logger.LogInformation("HotReload Initiated");
            await _restClient.GetAsync(null);
            _logger.LogInformation("HotReload Completed");
        }
    }
}
