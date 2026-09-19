using MicroKernel.Core;

namespace MicroKernel.Plugins;

public sealed class EchoPlugin : IPlugin
{
    public string Name => "Echo";
    public string Description => "Echoes or reverses text";
    public IReadOnlyList<string> Commands => ["echo", "reverse"];

    public string? Execute(string command, string[] args)
    {
        var text = string.Join(" ", args);
        return command switch
        {
            "echo"    => text,
            "reverse" => new string(text.Reverse().ToArray()),
            _         => null
        };
    }
}
