using ApplicationGateway.Application.Contracts.Infrastructure;
using ApplicationGateway.Application.Exceptions;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationGateway.Infrastructure.Redis
{
    [ExcludeFromCodeCoverage]
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ISubscriber _subscriber;
        private readonly IDatabase _database;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _subscriber = _redis.GetSubscriber();
            _database = _redis.GetDatabase();
        }

        public async Task PublishAsync(string message)
        {
            await _subscriber.PublishAsync("policy", message);
        }

        public async Task<string> GetAsync(string policyId)
        {
            if (!await _database.KeyExistsAsync(policyId))
            {
                throw new NotFoundException($"Policy with id:", policyId);
            }

            return await _database.StringGetAsync(policyId);
        }

        public async Task CreateUpdateAsync(string policyId, JObject transformedObject, string operation)
        {
            if(operation == "update")
            {
                if (!await _database.KeyExistsAsync(policyId))
                {
                    throw new NotFoundException($"Policy with id:", policyId);
                }
            }

            await _database.StringSetAsync(policyId, transformedObject.ToString());
            transformedObject.Add("policyId", policyId);
            transformedObject.Add("operation", operation);
            await PublishAsync(transformedObject.ToString());
        }

        public async Task DeleteAsync(string policyId)
        {
            if(!await _database.KeyExistsAsync(policyId))
            {
                throw new NotFoundException($"Policy with id:", policyId);
            }

            await _database.KeyDeleteAsync(policyId);

            JObject transformedObject = new JObject()
            {
                ["policyId"] = policyId.ToString(),
                ["operation"] = "delete"
            };
            await PublishAsync(transformedObject.ToString());
        }
    }
}
