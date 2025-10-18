

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public abstract class StateCommand<TGrain>(
    IClusterClient clusterClient,
    ILogger<StateCommand<TGrain>> logger) : ICommand
    where TGrain : ICreateable
{
    [CommandOption("grainId", 'g')]
    public string GrainId { get; init; } = "grain1-0";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<TGrain>(GrainId);

        var initState = await grain.GetState();
        logger.LogDebug("Init state: {@State}", initState);
        await console.Output.WriteLineAsync($"Init State: {JsonConvert.SerializeObject(initState, Formatting.Indented)}");

        await grain.Create();

        var state = await grain.GetState();
        logger.LogDebug("State after create: {@State}", state);
        await console.Output.WriteLineAsync($"State after create: {JsonConvert.SerializeObject(state, Formatting.Indented)}");
    }
}

[Command("state-as-class")]
public class StateAsClassCommand(
    IClusterClient clusterClient,
    ILogger<StateAsClassCommand> logger) : StateCommand<IGrainWithStateAsClass>(clusterClient, logger);

[Command("state-as-struct")]
public class StateAsStructCommand(
    IClusterClient clusterClient,
    ILogger<StateAsStructCommand> logger) : StateCommand<IGrainWithStateAsStruct>(clusterClient, logger);

[Command("state-as-struct-with-private-fields")]
public class StateAsStructWithPrivateFieldsCommand(
    IClusterClient clusterClient,
    ILogger<StateAsStructWithPrivateFieldsCommand> logger) : StateCommand<IGrainWithStateAsStructWithPrivateFields>(clusterClient, logger);
