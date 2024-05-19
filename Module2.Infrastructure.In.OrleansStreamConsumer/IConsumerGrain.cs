namespace Module2.Infrastructure.In.OrleansStreamConsumer;

public interface IConsumerGrain : IGrainWithGuidKey
{
    Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse);

    Task StopConsuming();

    Task<int> GetNumberConsumed();
}
