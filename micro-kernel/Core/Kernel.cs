namespace MicroKernel.Core;

// The kernel knows only about IPlugin — never about concrete plugins.
// Adding a new plugin never requires touching this file.
public sealed class Kernel
{
    private readonly List<IPlugin> _plugins = [];
    private readonly Dictionary<string, IPlugin> _routing = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<IPlugin> Plugins => _plugins;

    public void Register(IPlugin plugin)
    {
        _plugins.Add(plugin);
        foreach (var cmd in plugin.Commands)
            _routing[cmd] = plugin;
    }

    public string? Dispatch(string command, string[] args)
    {
        return _routing.TryGetValue(command, out var plugin)
            ? plugin.Execute(command, args)
            : null;
    }

    public bool Handles(string command) => _routing.ContainsKey(command);
}
