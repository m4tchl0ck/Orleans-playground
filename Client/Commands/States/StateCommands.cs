

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public abstract class StateCommand<TGrain, TState>(
    IClusterClient clusterClient,
    ILogger<StateCommand<TGrain, TState>> logger) : ICommand
    where TGrain : ICreateable<TState>
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<TGrain>("grain1-0");

        await grain.Create();

        var state = await grain.GetState();

        logger.LogDebug("Readed state: {@State}", state);

        await console.Output.WriteLineAsync($"State: {JsonConvert.SerializeObject(state, Formatting.Indented)}");
    }
}

[Command("state-as-class")]
public class StateAsClassCommand(
    IClusterClient clusterClient,
    ILogger<StateAsClassCommand> logger) : StateCommand<IGrainWithStateAsClass, StateAsClass>(clusterClient, logger);


[Command("state-as-struct")]
public class StateAsStructCommand(
    IClusterClient clusterClient,
    ILogger<StateAsStructCommand> logger) : StateCommand<IGrainWithStateAsStruct, StateAsStruct>(clusterClient, logger);