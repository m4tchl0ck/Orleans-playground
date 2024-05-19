using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace Module2.Infrastructure.In.OrleansStreamConsumer;

public class ConsumerGrain : Grain, IConsumerGrain
{
    private IAsyncObservable<int> consumer;
    internal int numConsumedItems;
    internal ILogger logger;
    private StreamSubscriptionHandle<int>? consumerHandle;

    public ConsumerGrain(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger($"{this.GetType().Name}-{this.IdentityString}");
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("OnActivateAsync");
        numConsumedItems = 0;
        consumerHandle = null;
        return Task.CompletedTask;
    }

    public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
    {
        logger.LogInformation("BecomeConsumer");
        IStreamProvider streamProvider = this.GetStreamProvider(providerToUse);
        consumer = streamProvider.GetStream<int>(streamNamespace, streamId);
        consumerHandle = await consumer.SubscribeAsync(Consume);
    }

    private async Task Consume(int item, StreamSequenceToken token)
    {
        logger.LogInformation("OnNextAsync(item={Item}, token={Token})", item, token != null ? token.ToString() : "null");
        numConsumedItems++;
        await Task.CompletedTask;
    }

    public async Task StopConsuming()
    {
        logger.LogInformation("StopConsuming");
        if (consumerHandle != null)
        {
            await consumerHandle.UnsubscribeAsync();
            consumerHandle = null;
        }
    }

    public Task<int> GetNumberConsumed()
    {
        return Task.FromResult(numConsumedItems);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        logger.LogInformation("OnDeactivateAsync");
        return Task.CompletedTask;
    }
}
