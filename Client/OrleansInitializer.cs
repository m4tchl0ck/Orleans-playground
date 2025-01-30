using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

public static class OrleansInitializer
{
    public static IHostBuilder UseClusterClient(this IHostBuilder hostBuilder)
        => hostBuilder
            .UseOrleansClient(clientBuilder =>
                clientBuilder
                    .ConfigureServices(services =>
                        services
                            .AddOptions<ClusterOptions>()
                                .BindConfiguration(nameof(ClusterOptions)))
                    .UseLocalhostClustering(gatewayPort: 30001))
            .UseConsoleLifetime();
}
