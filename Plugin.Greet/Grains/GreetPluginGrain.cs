public class GreetPluginGrain : Grain, IPluginGrain
{
    private static readonly PluginDescriptor _descriptor = new(
        Name: "Greeter",
        Description: "Greets and farewells people by name",
        Commands: ["greet", "bye"]);

    public Task<PluginDescriptor> Describe() => Task.FromResult(_descriptor);

    public Task<string?> Execute(string command, string[] args)
    {
        var name = args.Length > 0 ? string.Join(" ", args) : "stranger";
        return Task.FromResult<string?>(command switch
        {
            "greet" => $"Hello, {name}!",
            "bye"   => $"Goodbye, {name}!",
            _       => null
        });
    }
}
