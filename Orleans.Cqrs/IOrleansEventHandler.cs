using Orleans.Streams;

namespace Orleans.Cqrs;

public interface IOrleansEventHandler<in TEvent>
    where TEvent : IOrleansEvent
{
    Task Handle(TEvent @event, StreamSequenceToken? token = null);
}
