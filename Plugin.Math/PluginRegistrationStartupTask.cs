using Microsoft.Extensions.Logging;
using Orleans.Runtime;

public class PluginRegistrationStartupTask(
    IGrainFactory grainFactory,
    ILogger<PluginRegistrationStartupTask> logger) : IStartupTask
{
    public async Task Execute(CancellationToken ct)
    {
        var plugin = grainFactory.GetGrain<IPluginGrain>("math", grainClassNamePrefix: "MathPlugin");
        var descriptor = await plugin.Describe();

        var kernel = grainFactory.GetGrain<IKernelGrain>(0);
        await kernel.RegisterPlugin("math", "MathPlugin", descriptor);

        logger.LogInformation("Registered plugin: {Plugin}", descriptor.Name);
    }
}
