using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Module1.Infeastructure.Out.OrleansStreamProducer;
using Module2.Infrastructure.In.OrleansStreamConsumer;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .ConfigureLogging(logging => logging.AddConsole());
        silo.AddMemoryStreams("StreamProvider")
            .AddMemoryGrainStorage("PubSubStore");
    })
    .UseConsoleLifetime();

using IHost host = builder.Build();

await host.StartAsync();

IClusterClient client = host.Services.GetRequiredService<IClusterClient>();

var producer = client.GetGrain<IProducerGrain>(Guid.Empty);
await producer.BecomeProducer(Guid.Empty, "stream.namespace", "StreamProvider");
await producer.StartPeriodicProducing();


var consumer = client.GetGrain<IConsumerGrain>(Guid.Empty);
await consumer.BecomeConsumer(Guid.Empty, "stream.namespace", "StreamProvider");

Console.WriteLine($"""


    Press any key to exit...
    """);

Console.ReadKey();

await producer.StopPeriodicProducing();
await consumer.StopConsuming();

await host.StopAsync();