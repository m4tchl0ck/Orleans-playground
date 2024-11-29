

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

        var logger = loggerFactory.CreateLogger<IInitService>();

        var grainWithClassAsState = grainFactory.GetGrain<IGrainWithClassAsState>("first");
        await grainWithClassAsState.Create();

        var state = await grainWithClassAsState.GetState();
        logger.LogDebug("State os {grainType}: {@State}", nameof(IGrainWithClassAsState), state);
    }
}