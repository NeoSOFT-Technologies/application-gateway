using ApplicationGateway.Application.Exceptions;
using ApplicationGateway.Application.Helper;
using ApplicationGateway.Application.Models.Tyk;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationGateway.Worker;
[ExcludeFromCodeCoverage]
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private readonly TykConfiguration _tykConfiguration;
    private readonly RestClient<string> _restClient;
    private readonly Dictionary<string, string> _headers;

    public Worker(ILogger<Worker> logger, IConnectionMultiplexer redis, IOptions<TykConfiguration> tykConfiguration)
    {
        _logger = logger;
        _redis = redis;
        _subscriber = _redis.GetSubscriber();
        _tykConfiguration = tykConfiguration.Value;
        _headers = new Dictionary<string, string>()
            {
                { "x-tyk-authorization", _tykConfiguration.Secret }
            };
        _restClient = new RestClient<string>(_tykConfiguration.Host, "/tyk/reload/group", _headers);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscriber.Subscribe("policy").OnMessage(async message =>
        {
            _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}] policy");
            JObject messageObject = JObject.Parse(message.Message);
            switch (messageObject["operation"].Value<string>())
            {
                case "create":
                    messageObject.Remove("operation");
                    await CreatePolicy(messageObject);
                    break;
                case "update":
                    messageObject.Remove("operation");
                    await UpdateDeletePolicyById(messageObject["policyId"].Value<string>(), messageObject);
                    break;
                case "delete":
                    messageObject.Remove("operation");
                    await UpdateDeletePolicyById(messageObject["policyId"].Value<string>());
                    break;
                default:
                    break;
            }
            await _restClient.GetAsync(null);
        }); 
    }

    public async Task<string> ReadPolicies(string policiesFolderPath)
    {
        _logger.LogInformation($"{policiesFolderPath}");
        if (!Directory.Exists(policiesFolderPath))
        {
            _logger.LogInformation($"Folder doesn't exist {Directory.GetCurrentDirectory()}");
            Directory.CreateDirectory(policiesFolderPath);
        }
        if (!File.Exists($@"{policiesFolderPath}/policies.json"))
        {
            _logger.LogInformation($"File doesn't exist {Directory.GetCurrentDirectory()}");
            _logger.LogInformation($@"{policiesFolderPath}/policies.json");
            StreamWriter sw = File.CreateText($@"{policiesFolderPath}/policies.json");
            await sw.WriteLineAsync("{}");
            sw.Dispose();
        }
        return await File.ReadAllTextAsync($@"{policiesFolderPath}/policies.json");
    }

    public async Task WritePolicies(string policiesFolderPath, string content)
    {
        _logger.LogInformation($"{policiesFolderPath}");
        _logger.LogInformation($"{Directory.GetCurrentDirectory()}");
        await File.WriteAllTextAsync($@"{policiesFolderPath}/policies.json", content);
    }

    public async Task CreatePolicy(JObject transformedObject)
    {
        string policiesJson = await ReadPolicies(_tykConfiguration.PoliciesFolderPath);
        JObject policiesObject = JObject.Parse(policiesJson);
        string policyId = transformedObject["policyId"].Value<string>();
        transformedObject.Remove("policyId");
        policiesObject.Add(policyId, transformedObject);

        await WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
    }

    public async Task UpdateDeletePolicyById(string policyId, JObject transformedObject = null)
    {
        string policiesJson = await ReadPolicies(_tykConfiguration.PoliciesFolderPath);
        JObject policiesObject = JObject.Parse(policiesJson);

        if (!policiesObject.ContainsKey(policyId))
        {
            throw new NotFoundException($"Policy with id:", policyId);
        }

        policiesObject.Remove(policyId);
        if (transformedObject is not null)
        {
            transformedObject.Remove("policyId");
            policiesObject.Add(policyId, transformedObject);
        }

        await WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
    }

    private async Task HotReload()
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("x-tyk-authorization", "foo");
            await client.GetAsync("http://localhost:8080/tyk/reload/group");
        }
    }
}
