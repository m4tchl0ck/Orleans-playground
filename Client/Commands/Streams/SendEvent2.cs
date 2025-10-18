using System.Xml;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("send-event2")]
public class SendEvent2(
    IClusterClient clusterClient) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var streamProvider = clusterClient.GetStreamProvider(StreamConstants.ProviderName);
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