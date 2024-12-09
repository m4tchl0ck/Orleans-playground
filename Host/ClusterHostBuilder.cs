using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Serialization.Exceptions;
using Serilog;

public static class ClusterHostBuilder
{
    public static IHostBuilder CreateClusterHostBuilder(string[] args)
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
                    },
                    writeToProviders: true)
                    .AddHostedService<StartService>()
            )
            .UseOrleans(silo =>
            {
                silo
                    .AddDynamoDBGrainStorageAsDefault(
                        configureOptions: dynamoDbOptions =>
                        {
                            dynamoDbOptions.TableName = "Monolith-Persistence";
                            dynamoDbOptions.CreateIfNotExists = true;
                            dynamoDbOptions.UseProvisionedThroughput = false;
                            dynamoDbOptions.ReadCapacityUnits = 5;
                            dynamoDbOptions.WriteCapacityUnits = 10;
                            dynamoDbOptions.Service = "eu-west-1";
                        }
                    )
                    .Configure<ClusterOptions>(
                        clusterOptions =>
                        {
                            clusterOptions.ClusterId = "Monolith-Cluster";
                            clusterOptions.ServiceId = "Monolith-Service";
                        }
                    )
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
                    )
                    .AddDynamoDBGrainStorage(
                        name: "PubSubStore",
                        configureOptions: dynamoDbOptions =>
                        {
                            dynamoDbOptions.TableName = "Monolith-PubSubStore";
                            dynamoDbOptions.CreateIfNotExists = true;
                            dynamoDbOptions.UseProvisionedThroughput = false;
                            dynamoDbOptions.ReadCapacityUnits = 5;
                            dynamoDbOptions.WriteCapacityUnits = 10;
                            dynamoDbOptions.Service = "eu-west-1";
                        }
                    )
                    .AddSqsStreams("Monolith-StreamProvider", sqsOptions =>
                    {
                        sqsOptions.ConnectionString = "Service=eu-west-1";
                    })
                    .AddGrainService<StatesService>();
            })
            .UseConsoleLifetime();
        return builder;
    }
}
