public interface IKernelGrain : IGrainWithIntegerKey
{
    // Called by each plugin silo on startup to announce itself.
    // grainTypePrefix is matched against the plugin grain's class name for routing.
    Task RegisterPlugin(string pluginKey, string grainTypePrefix, PluginDescriptor descriptor);

    Task<string?> Dispatch(string command, string[] args);
    Task<IReadOnlyList<PluginDescriptor>> GetLoadedPlugins();
}
