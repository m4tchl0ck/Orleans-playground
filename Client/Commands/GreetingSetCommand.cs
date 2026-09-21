using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;
using Orleans.Cqs;

[Command("greeting set")]
public partial class GreetingSetCommand(IOrleansBus bus) : ICommand
{
    [CommandParameter(0, Description = "Unique identifier for this greeting")]
    public required string Id { get; set; }

    [CommandParameter(1, Description = "Name to associate with this identifier")]
    public required string Name { get; set; }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        await bus.SendCommand(new SetGreetingNameCommand(Id, Name));
        await console.Output.WriteLineAsync($"Name '{Name}' set for '{Id}'.");
    }
}
