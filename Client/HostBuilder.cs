using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Serilog;

public static class HostBuilder
{
    public static IHostBuilder CreateClientHostBuilder(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configuration =>
                configuration
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", false)
            )
            .ConfigureLogging(logging =>
                logging.ClearProviders()
            )
            .ConfigureServices((builder, services) =>
                services.AddSerilog(
                    config =>
                    {
                        config.ReadFrom.Configuration(builder.Configuration);
                        config.Enrich.FromLogContext();
                        config.Enrich.WithProperty("Host", "client");
                    },
                    writeToProviders: true)
                    .AddHostedService<StartService>()
            ).
            UseOrleansClient(clientBuilder =>
            {
                clientBuilder
                    .Configure<ClusterOptions>(
                        clusterOptions =>
                        {
                            clusterOptions.ClusterId = "Monolith-Cluster";
                            clusterOptions.ServiceId = "Monolith-Service";
                        })
                    .UseDynamoDBClustering(
                        dynamoDbOptions =>
                        {
                            dynamoDbOptions.TableName = "Monolith-Clustering";
                            dynamoDbOptions.CreateIfNotExists = true;
                            dynamoDbOptions.UseProvisionedThroughput = false;
                            dynamoDbOptions.ReadCapacityUnits = 5;
                            dynamoDbOptions.WriteCapacityUnits = 10;
                            dynamoDbOptions.Service = "eu-west-1";
                        }
                    );
            })
            .UseConsoleLifetime();
        return builder;
    }
}
