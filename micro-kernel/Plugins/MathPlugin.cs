using MicroKernel.Core;

namespace MicroKernel.Plugins;

public sealed class MathPlugin : IPlugin
{
    public string Name => "Calculator";
    public string Description => "Basic arithmetic: add, sub, mul, div <a> <b>";
    public IReadOnlyList<string> Commands => ["add", "sub", "mul", "div"];

    public string? Execute(string command, string[] args)
    {
        if (args.Length < 2
            || !double.TryParse(args[0], out var a)
            || !double.TryParse(args[1], out var b))
            return $"Usage: {command} <a> <b>";

        return command switch
        {
            "add" => $"{a} + {b} = {a + b}",
            "sub" => $"{a} - {b} = {a - b}",
            "mul" => $"{a} × {b} = {a * b}",
            "div" when b == 0 => "Error: division by zero",
            "div" => $"{a} ÷ {b} = {a / b}",
            _     => null
        };
    }
}
