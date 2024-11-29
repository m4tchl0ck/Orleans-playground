using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

namespace Monolith.Producer;

public class ProducerGrain : Grain, IProducerGrain
{
    private IAsyncStream<int> producer;
    private int numProducedItems;
    private IDisposable? producerTimer;
    internal ILogger logger;
    internal readonly static string RequestContextKey = "RequestContextField";
    internal readonly static string RequestContextValue = "JustAString";

    public ProducerGrain(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger($"{this.GetType().Name}-{this.IdentityString}");
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("OnActivateAsync");
        numProducedItems = 0;
        return Task.CompletedTask;
    }

    public Task BecomeProducer(Guid streamId, string streamNamespace, string providerToUse)
    {
        logger.LogInformation("BecomeProducer");
        IStreamProvider streamProvider = this.GetStreamProvider(providerToUse);
        producer = streamProvider.GetStream<int>(streamNamespace, streamId);
        return Task.CompletedTask;
    }

    public Task StartPeriodicProducing()
    {
        logger.LogInformation("StartPeriodicProducing");
        producerTimer = base.RegisterTimer(TimerCallback, new object(), TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public Task StopPeriodicProducing()
    {
        logger.LogInformation("StopPeriodicProducing");
        producerTimer?.Dispose();
        producerTimer = null;
        return Task.CompletedTask;
    }

    public Task<int> GetNumberProduced()
    {
        logger.LogInformation("GetNumberProduced {Count}", numProducedItems);
        return Task.FromResult(numProducedItems);
    }

    public Task ClearNumberProduced()
    {
        numProducedItems = 0;
        return Task.CompletedTask;
    }

    public Task Produce()
    {
        return Fire();
    }

    private Task TimerCallback(object state)
    {
        return producerTimer != null ? Fire() : Task.CompletedTask;
    }

    private async Task Fire([CallerMemberName] string? caller = null)
    {
        RequestContext.Set(RequestContextKey, RequestContextValue);
        await producer.OnNextAsync(numProducedItems);
        numProducedItems++;
        logger.LogInformation("{Caller} (item count={Count})", caller, numProducedItems);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        logger.LogInformation("OnDeactivateAsync");
        return Task.CompletedTask;
    }
}
