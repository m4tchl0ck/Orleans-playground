

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
    }

    private async Task CreateGrain<TGrain, TState>() where TGrain : ICreateable<TState>
    {
        var logger = loggerFactory.CreateLogger<IInitService>();

        var grainWithClassAsState = grainFactory.GetGrain<TGrain>("first");
        await grainWithClassAsState.Create();

        var state = await grainWithClassAsState.GetState();
        logger.LogDebug("Readed state: {@State}", state);
    }
}