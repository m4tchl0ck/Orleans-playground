using Orleans;

namespace Module1.Infeastructure.Out.OrleansStreamProducer;

public interface IProducerGrain : IGrainWithGuidKey
    {
        Task BecomeProducer(Guid streamId, string streamNamespace, string providerToUse);

        Task StartPeriodicProducing();

        Task StopPeriodicProducing();

        Task<int> GetNumberProduced();

        Task ClearNumberProduced();
        Task Produce();
    }
