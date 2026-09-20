namespace Orleans.Cqs;

public interface IOrleansBus
{
    Task SendCommand(IOrleansCommand command, CancellationToken cancellationToken = default);

    Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IQueryResult;
}
