using Microsoft.Extensions.Hosting;

IHostBuilder clusterHostBuilder = ClusterHostBuilder.CreateClusterHostBuilder(args);

using IHost clusterHost = clusterHostBuilder.Build();

await clusterHost.RunAsync();
