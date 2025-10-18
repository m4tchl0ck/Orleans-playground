
using System.Diagnostics;
using Microsoft.Extensions.Logging;

internal class DirtyStateSimulation(
    ILogger<DirtyStateSimulation> logger) : Grain<DirtyStateSimulationState>, IDirtyStateSimulation
{
    public async Task SetValue(int value)
    {
        State.Value = value;
        await WriteStateAsync();
    }

    public Task<int> GetValue() => Task.FromResult(State.Value);

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", (context.Grain as IAddressable)?.GetPrimaryKeyString());
        activity?.SetTag("GrainType", context.InterfaceName);
        activity?.SetTag("GrainMethodName", context.InterfaceMethod.Name);

        var oldState = State;
        try
        {
            await context.Invoke();
        }
        catch (Exception ex)
        {
            State = oldState;
            logger.LogError(ex, "Error invoking grain method {MethodName}", context.MethodName);
            activity?.SetStatus(ActivityStatusCode.Ok);
            throw;
        }

        activity?.SetTag("Result", context.Result);
    }
}

internal class DirtyStateSimulationState
{
    public int Value { get; set; }
}