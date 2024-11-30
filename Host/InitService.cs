

using Microsoft.Extensions.Logging;
using Monolith.State;
using Orleans.Services;

public interface IInitService : IGrainService;

public class InitService(
    GrainId grainId,
    Silo silo,
    ILoggerFactory loggerFactory,
    IGrainFactory grainFactory) : GrainService(grainId, silo, loggerFactory), IInitService
{
    public override async Task Start()
    {
        await base.Start();
        await CreateGrain<IGrainWithClassAsState, ClassAsState>();
        await CreateGrain<IGrainWithStructAsState, StructAsState>();
        // GrainWithStructWithPrivateFieldsAsState doesn't work as expected
        await CreateGrain<IGrainWithStructWithPrivateFieldsAsState, StructWithPrivateFieldsAsState>();        
    }

    private async Task CreateGrain<TGrain, TState>()
        where TGrain : ICreateable<TState> 
        where TState : IAgeable
    {
        var logger = loggerFactory.CreateLogger<IInitService>();

        var grain = grainFactory.GetGrain<TGrain>("first");
        
        var state = await grain.GetState();
        logger.LogDebug("Readed state: {@State} age {age} before create", state, state.GetAge());

        await grain.Create();

        state = await grain.GetState();
        logger.LogDebug("Readed state: {@State} age {age} after create", state, state.GetAge());
    }
}