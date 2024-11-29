

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


[Command("state-as-class")]
public class StateAsClassCommand(
    IClusterClient clusterClient,
    ILogger<CheckDirtyStateCommand> logger) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IGrainWithStateAsClass>("grain1-0");

        await grain.Create();

        var state = await grain.GetState();

        logger.LogDebug("Readed state: {@State}", state);

        await console.Output.WriteLineAsync($"State: {JsonConvert.SerializeObject(state, Formatting.Indented)}");
    }
}