using System.Xml;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("send-event")]
public class SendEvent(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("event", 'e')]
    public Events Event { get; init; } = Events.SomeEvent1;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var streamProvider = clusterClient.GetStreamProvider(StreamConstants.ProviderName);
        switch (Event)
        {
            case Events.SomeEvent1:
                {
                    var stream = streamProvider.GetStream<SomeEvent1>(StreamId.Create(StreamConstants.Namespace1, "grain-1"));

                    var @event = new SomeEvent1
                    {
                        EventData = "Hello, Stream!"
                    };
                    await stream.OnNextAsync(@event);

                    await console.Output.WriteLineAsync($"Event sent: {@event.EventData}");
                    return;
                }
            case Events.SomeEvent2:
                {
                    var stream = streamProvider.GetStream<SomeEvent2>(StreamId.Create(StreamConstants.Namespace2, "grain-1"));

                    var @event = new SomeEvent2
                    {
                        EventData = "Hello, Stream!"
                    };
                    await stream.OnNextAsync(@event);

                    await console.Output.WriteLineAsync($"Event sent: {@event.EventData}");
                    return;
                }
        }
    }

    public enum Events
    {
        SomeEvent1,
        SomeEvent2
    }
}