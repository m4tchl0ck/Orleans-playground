using System.Xml;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("send-event")]
public class SendEvent(
    IClusterClient clusterClient) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var streamProvider = clusterClient.GetStreamProvider(StreamConstants.ProviderName);
        var stream = streamProvider.GetStream<SomeEvent>(StreamId.Create(StreamConstants.Namespace, "grain-1"));

        var @event = new SomeEvent
        {
            EventData = "Hello, Stream!"
        };
        await stream.OnNextAsync(@event);

        await console.Output.WriteLineAsync($"Event sent: {@event.EventData}");
    }
}