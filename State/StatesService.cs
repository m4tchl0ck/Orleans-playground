

using Microsoft.Extensions.Logging;
using Monolith.State;
using Orleans.Runtime;
using Orleans.Services;

public interface IStatesService : IGrainService;

public class StatesService(
    GrainId grainId,
    Silo silo,
    ILoggerFactory loggerFactory,
    IGrainFactory grainFactory) : GrainService(grainId, silo, loggerFactory), IStatesService
{
    public override async Task Start()
    {
        await base.Start();
        await CreateGrain<IGrainWithClassAsState, ClassAsState>();
        await CreateGrain<IGrainWithStructAsState, StructAsState>();
        // GrainWithStructWithPrivateFieldsAsState doesn't work as expected
        await CreateGrain<IGrainWithStructWithPrivateFieldsAsState, StructWithPrivateFieldsAsState>();
        await CreateGrain<IGrainWithClassWithPrivateFieldsAsState, ClassWithPrivateFieldsAsState>();
        await CreateGrain<IGrainWithRecordWithPrivateFieldsAsState, RecordWithPrivateFieldsAsState>();
    }

    private async Task CreateGrain<TGrain, TState>()
        where TGrain : ICreateable<TState> 
        where TState : IAgeable
    {
        var logger = loggerFactory.CreateLogger<IStatesService>();

        var grain = grainFactory.GetGrain<TGrain>("first");

        var state = await grain.GetState();
        logger.LogDebug("Readed state: {@State} age {age} before create", state, state.GetAge());

        await grain.Create();

        state = await grain.GetState();
        logger.LogDebug("Readed state: {@State} age {age} after create", state, state.GetAge());
    }
}
