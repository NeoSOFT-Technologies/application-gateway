using ApplicationGateway.Application.Contracts.Infrastructure.SnapshotWrapper;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Application.Contracts.Persistence;
using ApplicationGateway.Persistence.Repositories;
using ApplicationGateway.Persistence.Repositories.DtoRepositories;
using ApplicationGateway.Persistence.SnapshotWrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("ApplicationConnectionString")));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITransformerRepository, TransformerRepository>();
            services.AddScoped<ISnapshotRepository, SnapshotRepository>();
            services.AddScoped<ISnapshotService, SnapshotService>();
            services.AddScoped<IApiRepository, ApiRepository>();
            services.AddScoped<IKeyRepository, KeyRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            return services;
        }
    }

}
