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
                    .AddTransient<CheckDirtyStateCommand>()
                    .AddTransient<SendEvent1>()
                    .AddTransient<SendEvent2>()
                    .AddTransient<StateAsClassCommand>()
                    .AddTransient<StateAsStructCommand>()
                    .AddTransient<StateAsStructWithPrivateFieldsCommand>()
                    .AddTransient<StateAsClassWithPrivateFieldsCommand>()
                    .AddTransient<StateAsRecordWithPrivateFieldsCommand>()
                    .AddTransient<ThrowExceptionCommand>()
                    .AddTransient<ThrowCustomExceptionCommand>()
                    .AddTransient<ThrowCustomInheritExceptionCommand>()
                    .AddSingleton(sp => new CliApplicationBuilder()
                        .AddCommandsFromThisAssembly()
                        .UseTypeActivator(sp.GetRequiredService)
                        .Build()));
    }
}
