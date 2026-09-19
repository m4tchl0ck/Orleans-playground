using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

[Command("kernel")]
public partial class KernelReplCommand(IClusterClient clusterClient) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var kernel = clusterClient.GetGrain<IKernelGrain>(0);

        var plugins = await kernel.GetLoadedPlugins();
        if (plugins.Count == 0)
        {
            await console.Output.WriteLineAsync("No plugins registered. Start plugin silos first.");
            return;
        }

        await console.Output.WriteLineAsync($"Kernel connected — {plugins.Count} plugin(s) loaded:");
        foreach (var p in plugins)
            await console.Output.WriteLineAsync($"  · {p.Name,-12} {p.Description}");
        await console.Output.WriteLineAsync("Type 'help' for commands, 'exit' to quit.");
        await console.Output.WriteLineAsync();

        while (true)
        {
            await console.Output.WriteAsync("kernel> ");
            var line = await console.Input.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line)) continue;
            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

            if (line.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                var loaded = await kernel.GetLoadedPlugins();
                foreach (var p in loaded)
                    await console.Output.WriteLineAsync(
                        $"[{p.Name}]  {string.Join("  ", p.Commands)}");
                continue;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = await kernel.Dispatch(parts[0], parts[1..]);
            await console.Output.WriteLineAsync(
                result ?? $"Unknown command '{parts[0]}'. Type 'help'.");
        }
    }
}
