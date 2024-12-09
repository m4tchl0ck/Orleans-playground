using Microsoft.Extensions.Hosting;

await RunClusterHost(args);

static async Task RunClusterHost(string[] args)
{
    using IHost host = HostBuilder.CreateClusterHostBuilder(args).Build();

    await host.RunAsync();
}
