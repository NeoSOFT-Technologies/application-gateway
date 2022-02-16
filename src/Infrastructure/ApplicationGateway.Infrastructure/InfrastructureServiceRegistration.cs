using ApplicationGateway.Application.Contracts.Infrastructure;
using ApplicationGateway.Application.Contracts.Infrastructure.PolicyWrapper;
using ApplicationGateway.Application.Models.Cache;
using ApplicationGateway.Application.Models.Mail;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Infrastructure.Cache;
using ApplicationGateway.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using ApplicationGateway.Infrastructure.PolicyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.ApiWrapper;
using ApplicationGateway.Infrastructure.ApiWrapper;
using ApplicationGateway.Infrastructure.KeyWrapper;
using ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper;

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
            services.AddTransient<ICacheService, MemoryCacheService>();
            services.AddTransient<IPolicyService, TykPolicyService>();
            services.AddTransient<IApiService, TykApiService>();
            services.AddTransient<IKeyService, TykKeyService>();
            services.Configure<TykConfiguration>(configuration.GetSection("TykConfiguration"));
            services.AddSendGrid(options => { options.ApiKey = configuration.GetValue<string>("EmailSettings:ApiKey"); });
            return services;
        }
    }
}
