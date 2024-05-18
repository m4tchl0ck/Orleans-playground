using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


using IHost siloHost = Host
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
                    },
                    writeToProviders: true)
            )
    .UseOrleans()
    .Build();

await siloHost.RunAsync();
