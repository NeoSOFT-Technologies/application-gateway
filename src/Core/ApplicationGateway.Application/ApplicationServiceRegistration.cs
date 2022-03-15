using ApplicationGateway.Application.Helper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ApplicationGateway.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<FileOperator>();
            services.AddScoped<TemplateTransformer>();

            return services;
        }
    }
}
