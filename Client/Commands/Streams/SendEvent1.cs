using System.Xml;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("send-event1")]
public class SendEvent1(
    IClusterClient clusterClient) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var streamProvider = clusterClient.GetStreamProvider(StreamConstants.ProviderName);
        var stream = streamProvider.GetStream<SomeEvent1>(StreamId.Create(StreamConstants.Namespace1, "grain-1"));

        var @event = new SomeEvent1
        {
            EventData = "Hello, Stream!"
        };
        await stream.OnNextAsync(@event);

        await console.Output.WriteLineAsync($"Event sent: {@event.EventData}");
        return;
    }
}