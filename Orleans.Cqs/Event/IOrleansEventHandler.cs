using Orleans.Streams;

namespace Orleans.Cqs;

public interface IOrleansEventHandler<in TEvent>
    where TEvent : IOrleansEvent
{
    Task Handle(TEvent @event, StreamSequenceToken? token = null);
}
