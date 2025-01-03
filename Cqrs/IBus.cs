namespace Cqrs;

public interface IBus
{
    Task Send(ICommand command);
}
