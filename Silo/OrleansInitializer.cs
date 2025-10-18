using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Providers;

public static class OrleansInitializer
{
    public static IHostBuilder UseOrleans(this IHostBuilder hostBuilder)
        => hostBuilder
            .UseOrleans(silo =>
                silo
                    .ConfigureServices(services =>
                    {
                        services
                            .AddOptions<ClusterOptions>()
                                .BindConfiguration(nameof(ClusterOptions));
                        services
                            .AddOptions<DynamoDBStorageOptions>(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME)
                                .BindConfiguration(nameof(DynamoDBStorageOptions));
                    })
                    .UseLocalhostClustering(
                        siloPort: 11111,
                        gatewayPort: 30001)
                    .UseDashboard(o => {})
                    .AddActivityPropagation()
                    .AddMemoryGrainStorageAsDefault()
                    .AddDynamoDBGrainStorageAsDefault()
                    .AddSqsStreams(StreamConstants.ProviderName, static (SiloSqsStreamConfigurator options) =>
                    {
                        options.ConfigurePartitioning(1);
                        options.ConfigureStreamPubSub(Orleans.Streams.StreamPubSubType.ImplicitOnly);
                        options.ConfigureSqs(builder =>
                        {
                            builder.Configure(o => o.ConnectionString = "Service=us-east-1");
                        });
                    })
            )
            .UseConsoleLifetime();
}
