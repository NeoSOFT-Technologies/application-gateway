using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationGateway.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class HealthcheckExtensionRegistration
    {
        public static IServiceCollection AddHealthcheckExtensionService(this IServiceCollection services, IConfiguration configuration)
        {
            string tykUrl = configuration["TykConfiguration:Host"] + configuration["API:Tyk"];
            services.AddHealthChecks()
                        .AddNpgSql(configuration["ConnectionStrings:ApplicationConnectionString"], name: "PostgreSQL", tags: new[] {
                            "db",
                            "all"})
                        .AddRedis(configuration["ConnectionStrings:Redis"], name: "Redis")
                        .AddUrlGroup(new Uri(configuration["TykConfiguration:Host"] + configuration["API:Tyk"]), name:"Gateway", tags: new[] {
                            "tykGatewayUrl",
                            "all"
                        })
                        .AddUrlGroup(new Uri(configuration["API:KeyCloak"]), name: "KeyCloak", tags: new[] {
                            "keyCloakUrl",
                            "all"
                        });
            services.AddHealthChecksUI(opt =>
                    {
                        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                        opt.SetApiMaxActiveRequests(1); //api requests concurrency
                        opt.AddHealthCheckEndpoint("API", "/healthz"); //map health check api
                    }).AddPostgreSqlStorage(configuration["ConnectionStrings:HealthCheckConnectionString"]);
            return services;
        }
    }
}
