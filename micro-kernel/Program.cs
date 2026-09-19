using MicroKernel.Core;
using MicroKernel.Plugins;

var kernel = new Kernel();

// --- Plugin registration: the only place that knows about concrete plugins ---
kernel.Register(new GreetPlugin());
kernel.Register(new MathPlugin());
kernel.Register(new EchoPlugin());
kernel.Register(new NotePlugin());
// Adding a new plugin = one new line here + a new class. Kernel unchanged.

Console.WriteLine("=== Micro-Kernel Demo ===");
Console.WriteLine($"Plugins loaded: {kernel.Plugins.Count}");
foreach (var p in kernel.Plugins)
    Console.WriteLine($"  · {p.Name,-12} {p.Description}");
Console.WriteLine();
Console.WriteLine("Type 'help' for commands, 'exit' to quit.");
Console.WriteLine();

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(line)) continue;

    if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    if (line.Equals("help", StringComparison.OrdinalIgnoreCase))
    {
        foreach (var plugin in kernel.Plugins)
        {
            Console.WriteLine($"[{plugin.Name}]  {plugin.Description}");
            Console.WriteLine($"  commands: {string.Join("  ", plugin.Commands)}");
        }
        continue;
    }

    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var result = kernel.Dispatch(parts[0], parts[1..]);

    Console.WriteLine(result ?? $"Unknown command '{parts[0]}'. Type 'help'.");
}
