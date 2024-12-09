using Microsoft.Extensions.Hosting;

IHostBuilder clusterHostBuilder = HostBuilder.CreateClusterHostBuilder(args);
using IHost clusterHost = clusterHostBuilder.Build();

var clusterHostTask = clusterHost.RunAsync();

using IHost clientHost = HostBuilder.CreateClientHostBuilder(args).Build();
await Task.Delay(10000);

await clientHost.StartAsync();

await clusterHost.StopAsync();