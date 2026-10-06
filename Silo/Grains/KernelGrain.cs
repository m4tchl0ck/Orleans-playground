using Microsoft.Extensions.Logging;

// The kernel grain is the cluster-wide dispatcher.
// It knows only about IPluginGrain — never about concrete plugin types.
// Adding a new plugin silo never requires changing this file.
public class KernelGrain(ILogger<KernelGrain> logger) : Grain, IKernelGrain
{
    private readonly Dictionary<string, string> _commandToPlugin = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, (string grainTypePrefix, PluginDescriptor descriptor)> _plugins = new();

    public override Task OnActivateAsync(CancellationToken ct)
    {
        // Keep alive so in-memory registrations survive idle periods.
        this.DelayDeactivation(TimeSpan.MaxValue);
        return base.OnActivateAsync(ct);
    }

    public Task RegisterPlugin(string pluginKey, string grainTypePrefix, PluginDescriptor descriptor)
    {
        _plugins[pluginKey] = (grainTypePrefix, descriptor);
        foreach (var cmd in descriptor.Commands)
            _commandToPlugin[cmd] = pluginKey;

        logger.LogInformation("Plugin registered: {Plugin} ({Commands})",
            descriptor.Name, string.Join(", ", descriptor.Commands));

        return Task.CompletedTask;
    }

    public async Task<string?> Dispatch(string command, string[] args)
    {
        if (!_commandToPlugin.TryGetValue(command, out var pluginKey))
            return null;

        var (grainTypePrefix, _) = _plugins[pluginKey];
        var plugin = GrainFactory.GetGrain<IPluginGrain>(pluginKey, grainClassNamePrefix: grainTypePrefix);
        return await plugin.Execute(command, args);
    }

    public Task<IReadOnlyList<PluginDescriptor>> GetLoadedPlugins() =>
        Task.FromResult<IReadOnlyList<PluginDescriptor>>(
            _plugins.Values.Select(p => p.descriptor).ToList());
}
