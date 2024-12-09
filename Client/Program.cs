using Microsoft.Extensions.Hosting;

await RunClientHost(args);

static async Task RunClientHost(string[] args)
{
    using IHost host = HostBuilder.CreateClientHostBuilder(args).Build();

    await host.RunAsync();
}