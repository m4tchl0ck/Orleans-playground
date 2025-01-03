namespace Cqrs;

public interface ICommandHandler<T>  where T : ICommand
{
    Task Handle(T command);
}
