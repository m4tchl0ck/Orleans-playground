using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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
    .UseObservability()
    .UseOrleans()
    .Build();

await siloHost.RunAsync();
