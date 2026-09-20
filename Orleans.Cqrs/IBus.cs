namespace Orleans.Cqrs;

public interface IBus
{
    Task SendCommand(IOrleansCommand command, CancellationToken cancellationToken = default);

    Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IQueryResult;

    Task Publish(IOrleansEvent @event, CancellationToken cancellationToken = default);
}
