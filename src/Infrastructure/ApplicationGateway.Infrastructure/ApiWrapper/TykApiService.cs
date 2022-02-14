using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Domain.TykData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationGateway.Infrastructure.ApiWrapper
{
    public class TykApiService : IApiService
    {
        private readonly ILogger<TykApiService> _logger;
        private readonly FileOperator _fileOperator;
        private readonly TykConfiguration _tykConfiguration;
        private readonly RestClient<string> _restClient;
        private readonly Dictionary<string, string> _headers;

        public TykApiService(ILogger<TykApiService> logger, FileOperator fileOperator, IOptions<TykConfiguration> tykConfiguration)
        {
            _logger = logger;
            _fileOperator = fileOperator;
            _tykConfiguration = tykConfiguration.Value;
            _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
            _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/apis", _headers);
        }

        public async Task<Api> CreateApi(Api api)
        {
            _logger.LogInformation("CreateApi Initiated with {@Api}", api);
            api.ApiId = Guid.NewGuid();
            string requestJson = JsonConvert.SerializeObject(api);
            string transformed = await _fileOperator.Transform(requestJson, "CreateApiTransformer");

            #region Add ApiId to Api
            JObject transformedObject = JObject.Parse(transformed);
            transformedObject.Add("api_id", Guid.NewGuid());
            #endregion

            await _restClient.PostAsync(transformedObject);

            _logger.LogInformation("CreateApi Completed");
            return api;
        }
    }
}
