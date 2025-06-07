using CliFx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHost clientHost = Host
    .CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
        configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
    )
    .ConfigureLogging(logging =>
        logging.ClearProviders()
    )
    .UseObservability()
    .UseClusterClient()
    .UseCliFx()
    .Build();

await clientHost.StartAsync();

var cliApplication = clientHost.Services.GetRequiredService<CliApplication>();
await cliApplication.RunAsync(args);

await clientHost.StopAsync();