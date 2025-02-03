using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("say-hello")]
public class SayHelloCommand(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("greeting", 'g')]
    public string Greeting { get; init; } = "Hello, World!";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IHelloWorld>("grain1-0");
        var response = await grain.SayHello(Greeting);

        await console.Output.WriteLineAsync($"{nameof(grain.SayHello)} response: {response}");
    }
}