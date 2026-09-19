using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

[Command("say-hello")]
public partial class SayHelloCommand(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("greeting", 'g')]
    public string Greeting { get; set; } = "Hello, World!";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IHelloWorld>("grain1-0");
        var response = await grain.SayHello(Greeting);

        await console.Output.WriteLineAsync($"{nameof(grain.SayHello)} response: {response}");
    }
}