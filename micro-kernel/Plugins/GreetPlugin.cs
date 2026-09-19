using MicroKernel.Core;

namespace MicroKernel.Plugins;

public sealed class GreetPlugin : IPlugin
{
    public string Name => "Greeter";
    public string Description => "Greets and farewells people by name";
    public IReadOnlyList<string> Commands => ["greet", "bye"];

    public string? Execute(string command, string[] args)
    {
        var name = args.Length > 0 ? string.Join(" ", args) : "stranger";
        return command switch
        {
            "greet" => $"Hello, {name}!",
            "bye"   => $"Goodbye, {name}!",
            _       => null
        };
    }
}
