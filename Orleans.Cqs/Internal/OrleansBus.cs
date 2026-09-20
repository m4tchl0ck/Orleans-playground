using System.Reflection;

namespace Orleans.Cqs.Internal;

internal sealed class OrleansBus(IGrainFactory grainFactory) : IOrleansBus
{
    private static readonly MethodInfo SendCommandMethod =
        typeof(OrleansBus).GetMethod(nameof(SendTypedCommand), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private static readonly MethodInfo RunQueryMethod =
        typeof(OrleansBus).GetMethod(nameof(RunTypedQuery), BindingFlags.NonPublic | BindingFlags.Instance)!;

    public Task SendCommand(IOrleansCommand command, CancellationToken cancellationToken = default)
    {
        var gcts = new GrainCancellationTokenSource();
        using var reg = cancellationToken.Register(static s => ((GrainCancellationTokenSource)s!).Cancel(), gcts);
        return (Task)SendCommandMethod.MakeGenericMethod(command.GetType())
            .Invoke(this, [command, gcts.Token])!;
    }

    private Task SendTypedCommand<TCommand>(TCommand command, GrainCancellationToken cancellationToken)
        where TCommand : IOrleansCommand
    {
        var handler = command.GrainType is not null
            ? grainFactory.GetGrain<IOrleansCommandHandler<TCommand>>(global::Orleans.Runtime.GrainId.Create(command.GrainType, command.GrainId))
            : grainFactory.GetGrain<IOrleansCommandHandler<TCommand>>(command.GrainId);
        return handler.Handle(command, cancellationToken);
    }

    public Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IQueryResult
    {
        var gcts = new GrainCancellationTokenSource();
        using var reg = cancellationToken.Register(static s => ((GrainCancellationTokenSource)s!).Cancel(), gcts);
        return (Task<TResult>)RunQueryMethod.MakeGenericMethod(query.GetType(), typeof(TResult))
            .Invoke(this, [query, gcts.Token])!;
    }

    private Task<TResult> RunTypedQuery<TQuery, TResult>(TQuery query, GrainCancellationToken cancellationToken)
        where TQuery : IOrleansQuery<TResult>
        where TResult : IQueryResult
    {
        var handler = query.GrainType is not null
            ? grainFactory.GetGrain<IOrleansQueryHandler<TQuery, TResult>>(global::Orleans.Runtime.GrainId.Create(query.GrainType, query.GrainId))
            : grainFactory.GetGrain<IOrleansQueryHandler<TQuery, TResult>>(query.GrainId);
        return handler.Handle(query, cancellationToken);
    }
}
