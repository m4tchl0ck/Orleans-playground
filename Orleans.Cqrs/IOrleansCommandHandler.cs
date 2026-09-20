namespace Orleans.Cqrs;

public interface IOrleansCommandHandler<in TCommand> : IGrainWithStringKey
    where TCommand : IOrleansCommand
{
    Task Handle(TCommand command, GrainCancellationToken cancellationToken);
}
