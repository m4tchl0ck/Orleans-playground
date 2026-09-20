using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;
using Orleans.Cqs;

[Command("kernel")]
public partial class KernelReplCommand(IOrleansBus bus) : CliFx.ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var result = await bus.RunQuery(new GetPluginsQuery(Guid.NewGuid().ToString()));

        if (result.Plugins.Count == 0)
        {
            await console.Output.WriteLineAsync("No plugins registered. Start plugin silos first.");
            return;
        }

        await console.Output.WriteLineAsync($"Kernel connected — {result.Plugins.Count} plugin(s) loaded:");
        foreach (var p in result.Plugins)
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
                var loaded = await bus.RunQuery(new GetPluginsQuery(Guid.NewGuid().ToString()));
                foreach (var p in loaded.Plugins)
                    await console.Output.WriteLineAsync(
                        $"[{p.Name}]  {string.Join("  ", p.Commands)}");
                continue;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var executeResult = await bus.RunQuery(new ExecutePluginQuery(parts[0], parts[1..]));
            await console.Output.WriteLineAsync(
                executeResult.Output ?? $"Unknown command '{parts[0]}'. Type 'help'.");
        }
    }
}
