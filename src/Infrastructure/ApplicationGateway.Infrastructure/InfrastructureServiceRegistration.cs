using ApplicationGateway.Application.Contracts.Infrastructure;
using ApplicationGateway.Application.Contracts.Infrastructure.Gateway;
using ApplicationGateway.Application.Models.Cache;
using ApplicationGateway.Application.Models.Mail;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Infrastructure.Cache;
using ApplicationGateway.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using ApplicationGateway.Infrastructure.Gateway.Tyk;
using ApplicationGateway.Infrastructure.Redis;
using StackExchange.Redis;

namespace ApplicationGateway.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            services.AddMemoryCache();
            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddTransient<ICacheService, MemoryCacheService>();
            services.AddTransient<IRedisService, RedisService>();
            services.AddTransient<IPolicyService, TykPolicyService>();
            services.AddTransient<IApiService, TykApiService>();
            services.AddTransient<IBaseService, TykBaseService>();
            services.AddTransient<IKeyService, TykKeyService>();
            services.Configure<TykConfiguration>(configuration.GetSection("TykConfiguration"));
            services.AddSendGrid(options => { options.ApiKey = configuration.GetValue<string>("EmailSettings:ApiKey"); });
            return services;
        }
    }
}
