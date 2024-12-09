using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class StartService : BackgroundService
{
    private readonly IClusterClient _clusterClient;
    private readonly ILogger<StartService> _logger;

    public StartService(IClusterClient clusterClient, ILogger<StartService> logger)
    {
        _clusterClient = clusterClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(10000, stoppingToken);
        _logger.LogInformation("Starting...");

        var grain = _clusterClient.GetGrain<IThrowingGrain>("some-key");
        await grain.WithExceptionHandling(_logger, async g => await g.ThrowException());
        await grain.WithExceptionHandling(_logger, async g => await g.ThrowCustomException());
        await grain.WithExceptionHandling(_logger, async g => await g.ThrowCustomInheridException());

        _logger.LogInformation("Finished");
    }
}
