using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

public static class OrleansInitializer
{
    public static IHostBuilder UseOrleans(this IHostBuilder hostBuilder)
        => hostBuilder
            .UseOrleans(silo =>
                silo
                    .ConfigureServices(services =>
                        services
                            .AddOptions<ClusterOptions>()
                                .BindConfiguration(nameof(ClusterOptions)))
                    .UseLocalhostClustering(
                        siloPort: 11111,
                        gatewayPort: 30001))
            .UseConsoleLifetime();
}
