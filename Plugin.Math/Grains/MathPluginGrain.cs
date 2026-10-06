public class MathPluginGrain : Grain, IPluginGrain
{
    private static readonly PluginDescriptor _descriptor = new(
        Name: "Calculator",
        Description: "Basic arithmetic: add, sub, mul, div <a> <b>",
        Commands: ["add", "sub", "mul", "div"]);

    public Task<PluginDescriptor> Describe() => Task.FromResult(_descriptor);

    public Task<string?> Execute(string command, string[] args)
    {
        if (args.Length < 2
            || !double.TryParse(args[0], out var a)
            || !double.TryParse(args[1], out var b))
            return Task.FromResult<string?>($"Usage: {command} <a> <b>");

        return Task.FromResult<string?>(command switch
        {
            "add" => $"{a} + {b} = {a + b}",
            "sub" => $"{a} - {b} = {a - b}",
            "mul" => $"{a} × {b} = {a * b}",
            "div" when b == 0 => "Error: division by zero",
            "div" => $"{a} ÷ {b} = {a / b}",
            _     => null
        });
    }
}
