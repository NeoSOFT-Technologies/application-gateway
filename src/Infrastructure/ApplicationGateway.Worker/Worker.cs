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
            JObject messageObject = JObject.Parse(message.Message);
            string operation = messageObject["operation"].Value<string>();
            _logger.LogInformation($"{operation} operation initiated");
            switch (operation)
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
            _logger.LogInformation($"{operation} operation completed");
        });
    }

    public async Task<string> ReadPolicies(string policiesFolderPath)
    {
        _logger.LogInformation("ReadPolicies initiated");
        if (!Directory.Exists(policiesFolderPath))
        {
            _logger.LogInformation("Policies folder doesn't exist");
            Directory.CreateDirectory(policiesFolderPath);
            _logger.LogInformation("Policies folder created");
        }
        if (!File.Exists($@"{policiesFolderPath}/policies.json"))
        {
            _logger.LogInformation("policies.json file doesn't exist");
            StreamWriter sw = File.CreateText($@"{policiesFolderPath}/policies.json");
            await sw.WriteLineAsync("{}");
            sw.Dispose();
            _logger.LogInformation("policies.json file created");
        }
        _logger.LogInformation("ReadPolicies completed");
        return await File.ReadAllTextAsync($@"{policiesFolderPath}/policies.json");
    }

    public async Task WritePolicies(string policiesFolderPath, string content)
    {
        _logger.LogInformation("WritePolicies initiated");
        await File.WriteAllTextAsync($@"{policiesFolderPath}/policies.json", content);
        _logger.LogInformation("WritePolicies completed");
    }

    public async Task CreatePolicy(JObject transformedObject)
    {
        _logger.LogInformation("CreatePolicy initiated");
        string policiesJson = await ReadPolicies(_tykConfiguration.PoliciesFolderPath);
        JObject policiesObject = JObject.Parse(policiesJson);
        string policyId = transformedObject["policyId"].Value<string>();
        transformedObject.Remove("policyId");
        policiesObject.Add(policyId, transformedObject);

        await WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
        _logger.LogInformation("CreatePolicy completed");
    }

    public async Task UpdateDeletePolicyById(string policyId, JObject transformedObject = null)
    {
        _logger.LogInformation($"UpdateDeletePolicyById initiated with policyId = {policyId} initiated");
        string policiesJson = await ReadPolicies(_tykConfiguration.PoliciesFolderPath);
        JObject policiesObject = JObject.Parse(policiesJson);

        if (!policiesObject.ContainsKey(policyId))
        {
            throw new NotFoundException($"Policy with id:", policyId);
        }

        policiesObject.Remove(policyId);
        if (transformedObject is not null)
        {
            _logger.LogInformation($"Policy update with policyId = {policyId} initiated");
            transformedObject.Remove("policyId");
            policiesObject.Add(policyId, transformedObject);
            _logger.LogInformation($"Policy with policyId = {policyId} updated");
        }
        else
        {
            _logger.LogInformation($"Policy with policyId = {policyId} deleted");
        }

        await WritePolicies(_tykConfiguration.PoliciesFolderPath, policiesObject.ToString());
        _logger.LogInformation($"UpdateDeletePolicyById initiated with policyId = {policyId} completed");
    }
}
