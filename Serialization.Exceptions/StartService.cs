using Microsoft.Extensions.Hosting;

namespace Serialization.Exceptions;

public class StartService : BackgroundService
{
    private readonly IClusterClient _clusterClient;

    public StartService(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(30000, stoppingToken);
        var grain = _clusterClient.GetGrain<IThrowingGrain>("some-key");
        try
        {
            await grain.ThrowException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
