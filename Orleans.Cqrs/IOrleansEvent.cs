using Orleans.Streams;

namespace Orleans.Cqrs;

public interface IOrleansEvent
{
    StreamId StreamId { get; }
    string StreamProvider { get; }
}
