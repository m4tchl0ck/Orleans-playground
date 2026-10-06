public class EchoPluginGrain : Grain, IPluginGrain
{
    private static readonly PluginDescriptor _descriptor = new(
        Name: "Echo",
        Description: "Echoes or reverses text",
        Commands: ["echo", "reverse"]);

    public Task<PluginDescriptor> Describe() => Task.FromResult(_descriptor);

    public Task<string?> Execute(string command, string[] args)
    {
        var text = string.Join(" ", args);
        return Task.FromResult<string?>(command switch
        {
            "echo"    => text,
            "reverse" => new string(text.Reverse().ToArray()),
            _         => null
        });
    }
}
