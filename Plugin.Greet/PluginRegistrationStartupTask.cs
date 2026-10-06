using Microsoft.Extensions.Logging;
using Orleans.Runtime;

// Mirrors kernel.Register(new GreetPlugin()) from the micro-kernel demo,
// but uses Orleans grain references instead of constructor injection.
public class PluginRegistrationStartupTask(
    IGrainFactory grainFactory,
    ILogger<PluginRegistrationStartupTask> logger) : IStartupTask
{
    public async Task Execute(CancellationToken ct)
    {
        var plugin = grainFactory.GetGrain<IPluginGrain>("greet", grainClassNamePrefix: "GreetPlugin");
        var descriptor = await plugin.Describe();

        var kernel = grainFactory.GetGrain<IKernelGrain>(0);
        await kernel.RegisterPlugin("greet", "GreetPlugin", descriptor);

        logger.LogInformation("Registered plugin: {Plugin}", descriptor.Name);
    }
}
