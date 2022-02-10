using ApplicationGateway.Application.Contracts.Infrastructure;
using ApplicationGateway.Application.Contracts.Infrastructure.Tyk;
using ApplicationGateway.Application.Models.Cache;
using ApplicationGateway.Application.Models.Mail;
using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Infrastructure.Cache;
using ApplicationGateway.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using ApplicationGateway.Infrastructure.PolicyWrapper;

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
            services.Configure<TykConfiguration>(configuration.GetSection("TykConfiguration"));
            services.AddSendGrid(options => { options.ApiKey = configuration.GetValue<string>("EmailSettings:ApiKey"); });
            return services;
        }
    }
}
