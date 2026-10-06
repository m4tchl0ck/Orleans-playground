using Orleans.Streams;

namespace Orleans.Cqs;

public interface IOrleansEvent
{
    StreamId StreamId { get; }
    string StreamProvider { get; }
}
