using Microsoft.Extensions.Logging;
using Orleans.Runtime;

public class PluginRegistrationStartupTask(
    IGrainFactory grainFactory,
    ILogger<PluginRegistrationStartupTask> logger) : IStartupTask
{
    public async Task Execute(CancellationToken ct)
    {
        var plugin = grainFactory.GetGrain<IPluginGrain>("echo", grainClassNamePrefix: "EchoPlugin");
        var descriptor = await plugin.Describe();

        var kernel = grainFactory.GetGrain<IKernelGrain>(0);
        await kernel.RegisterPlugin("echo", "EchoPlugin", descriptor);

        logger.LogInformation("Registered plugin: {Plugin}", descriptor.Name);
    }
}
