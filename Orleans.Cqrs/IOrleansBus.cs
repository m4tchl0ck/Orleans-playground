namespace Orleans.Cqrs;

public interface IOrleansBus
{
    Task SendCommand(IOrleansCommand command, GrainCancellationToken cancellationToken);

    Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, GrainCancellationToken cancellationToken)
        where TResult : IQueryResult;

    Task Publish(IOrleansEvent @event, CancellationToken cancellationToken = default);
}
