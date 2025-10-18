using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Streams.Core;

[ImplicitStreamSubscription(StreamConstants.Namespace)]
public class Consumer(ILogger<Consumer> logger) : Grain, IConsumer, IStreamSubscriptionObserver, IAsyncObserver<SomeEvent>
{
    public Task OnErrorAsync(Exception ex)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());
        activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

        logger.LogError(ex, "Error in stream subscription");
        return Task.CompletedTask;
    }

    public Task OnNextAsync(SomeEvent item, StreamSequenceToken? token = null)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());

        logger.LogInformation("Received event: {Event}", item);
        return Task.CompletedTask;
    }

    public Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        using var activity = ActivitySource.StartActivity();
        activity?.SetTag("GrainId", this.GetPrimaryKeyString());
        handleFactory.Create<SomeEvent>().ResumeAsync(this);

        logger.LogInformation("Subscribed to stream");
        return Task.CompletedTask;
    }
}