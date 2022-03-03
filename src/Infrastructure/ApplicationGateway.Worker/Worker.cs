using ApplicationGateway.Application.Exceptions;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace ApplicationGateway.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private readonly string _policiesFolderPath;

    public Worker(ILogger<Worker> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _subscriber = _redis.GetSubscriber();
        _policiesFolderPath = "C:\\Projects\\ApplicationGateway\\application-gateway\\tyk-gateway-docker\\tyk-gateway-docker\\policies";
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
                    await HotReload();
                    break;
                case "update":
                    messageObject.Remove("operation");
                    await UpdateDeletePolicyById(messageObject["policyId"].Value<string>(), messageObject);
                    await HotReload();
                    break;
                case "delete":
                    messageObject.Remove("operation");
                    await UpdateDeletePolicyById(messageObject["policyId"].Value<string>());
                    await HotReload();
                    break;
                default:
                    break;
            }
            //await Task.Delay(1000);
        }); 
    }

    public async Task<string> ReadPolicies(string policiesFolderPath)
    {
        if (!Directory.Exists(policiesFolderPath))
        {
            Directory.CreateDirectory(policiesFolderPath);
        }
        if (!File.Exists($@"{policiesFolderPath}\policies.json"))
        {
            StreamWriter sw = File.CreateText($@"{policiesFolderPath}\policies.json");
            await sw.WriteLineAsync("{}");
            sw.Dispose();
        }
        return await File.ReadAllTextAsync($@"{policiesFolderPath}\policies.json");
    }

    public async Task WritePolicies(string policiesFolderPath, string content)
    {
        await File.WriteAllTextAsync($@"{policiesFolderPath}\policies.json", content);
    }

    public async Task CreatePolicy(JObject transformedObject)
    {
        string policiesJson = await ReadPolicies(_policiesFolderPath);
        JObject policiesObject = JObject.Parse(policiesJson);
        string policyId = transformedObject["policyId"].Value<string>();
        transformedObject.Remove("policyId");
        policiesObject.Add(policyId, transformedObject);

        await WritePolicies(_policiesFolderPath, policiesObject.ToString());
    }

    public async Task UpdateDeletePolicyById(string policyId, JObject transformedObject = null)
    {
        string policiesJson = await ReadPolicies(_policiesFolderPath);
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

        await WritePolicies(_policiesFolderPath, policiesObject.ToString());
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
