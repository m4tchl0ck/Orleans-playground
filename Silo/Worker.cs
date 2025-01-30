using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker(
    ILogger<Worker> logger, 
    IGrainFactory grainFactory
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);
        logger.LogInformation("Worker running");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var grain = grainFactory.GetGrain<IHelloWorld>("grain1-0");
            var response = await grain.SayHello("Hello, World!");
            logger.LogInformation("{method} response: {response}", nameof(grain.SayHello), response);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
