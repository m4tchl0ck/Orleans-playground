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
                    .UseLocalhostClustering(gatewayPort: 30001)
                    .AddSqsStreams(StreamConstants.ProviderName, options =>
                    {
                        options.ConfigurePartitioning(1);
                        options.ConfigureStreamPubSub(Orleans.Streams.StreamPubSubType.ImplicitOnly);

                        options.ConfigureSqs(builder =>
                        {
                            builder.Configure(o => o.ConnectionString = "Service=us-east-1");
                        });
                    }))
            .UseConsoleLifetime();
}
