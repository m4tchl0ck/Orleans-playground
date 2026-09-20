namespace Orleans.Cqs;

public interface IOrleansQueryHandler<in TQuery, TResult> : IGrainWithStringKey
    where TQuery : IOrleansQuery<TResult>
    where TResult : IQueryResult
{
    Task<TResult> Handle(TQuery query, GrainCancellationToken cancellationToken);
}
