using ApplicationGateway.Worker;
using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
    })
    .Build();



await host.RunAsync();
