using CliFx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class CliFxInitializer
{
    public static IHostBuilder UseCliFx(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .ConfigureServices((host, collection) => 
                collection
                    .AddTransient<InteractiveCommand>()
                    .AddTransient<SayHelloCommand>()
                    .AddSingleton(sp => new CliApplicationBuilder()
                        .AddCommandsFromThisAssembly()
                        .UseTypeActivator(sp.GetRequiredService)
                        .Build()));
    }
}
