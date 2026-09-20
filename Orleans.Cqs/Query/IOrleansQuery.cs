namespace Orleans.Cqs;

public interface IOrleansQuery
{
    string GrainId { get; }
    string? GrainType => null;
}

public interface IOrleansQuery<out TResult> : IOrleansQuery
    where TResult : IQueryResult;
