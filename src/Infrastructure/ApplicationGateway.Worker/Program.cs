using ApplicationGateway.Application.Models.Tyk;
using ApplicationGateway.Worker;
using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.Configure<TykConfiguration>(configuration.GetSection("TykConfiguration"));
    })
    .Build();



await host.RunAsync();
