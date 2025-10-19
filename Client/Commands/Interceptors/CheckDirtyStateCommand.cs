using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("check-dirty-state")]
public class CheckDirtyStateCommand(
    IClusterClient clusterClient) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IDirtyStateSimulation>("grain1-0");
        await grain.SetValue(42);
        var value = await grain.GetValue();

        await console.Output.WriteLineAsync($"{nameof(grain.GetValue)} Value: {value}");
    }
}