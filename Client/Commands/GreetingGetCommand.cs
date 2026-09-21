using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;
using Orleans.Cqs;

[Command("greeting get")]
public partial class GreetingGetCommand(IOrleansBus bus) : ICommand
{
    [CommandParameter(0, Description = "Identifier to retrieve the greeting for")]
    public required string Id { get; set; }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var result = await bus.RunQuery(new GetGreetingQuery(Id));
        await console.Output.WriteLineAsync(result.Message);
    }
}
