using CliFx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

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
    .ConfigureServices((builder, services) =>
        services.AddSerilog(
            config =>
            {
                config.ReadFrom.Configuration(builder.Configuration);
                config.Enrich.FromLogContext();
                config.Enrich.WithProperty("Host", "client");
            },
            writeToProviders: true)
    )
    .UseClusterClient()
    .UseCliFx()
    .Build();
await clientHost.StartAsync();

var cliApplication = clientHost.Services.GetRequiredService<CliApplication>();
await cliApplication.RunAsync(args);

await clientHost.StopAsync();