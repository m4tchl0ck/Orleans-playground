using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Streams.Core;

[ImplicitStreamSubscription(StreamConstants.Namespace1)]
[ImplicitStreamSubscription(StreamConstants.Namespace2)]
public class Consumer(ILogger<Consumer> logger) : Grain, IConsumer, IStreamSubscriptionObserver, IAsyncObserver<SomeEvent1>, IAsyncObserver<SomeEvent2>
{
    public Task OnErrorAsync(Exception ex)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());
        activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

        logger.LogError(ex, "Error in stream subscription");
        return Task.CompletedTask;
    }

    public Task OnNextAsync(SomeEvent1 item, StreamSequenceToken? token = null)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());

        logger.LogInformation("Received event[{EventType}]: {Event}", nameof(SomeEvent1), item);
        return Task.CompletedTask;
    }

    public Task OnNextAsync(SomeEvent2 item, StreamSequenceToken? token = null)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());

        logger.LogInformation("Received event[{EventType}]: {Event}", nameof(SomeEvent2), item);
        return Task.CompletedTask;
    }

    public Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());

        logger.LogWarning("Subscribing to stream {StreamId}", handleFactory.StreamId.Namespace);

        if (handleFactory.StreamId == StreamId.Create(StreamConstants.Namespace1, this.GetPrimaryKeyString()))
        {
            handleFactory.Create<SomeEvent1>().ResumeAsync(this);
        }
        if (handleFactory.StreamId == StreamId.Create(StreamConstants.Namespace2, this.GetPrimaryKeyString()))
        {
            handleFactory.Create<SomeEvent2>().ResumeAsync(this);
        }

        logger.LogInformation("Subscribed to stream {StreamId}", handleFactory.StreamId);
        return Task.CompletedTask;
    }
}