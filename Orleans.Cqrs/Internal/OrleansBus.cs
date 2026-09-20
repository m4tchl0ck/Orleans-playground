using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Streams;

namespace Orleans.Cqrs.Internal;

internal sealed class OrleansBus(IGrainFactory grainFactory, IServiceProvider serviceProvider) : IOrleansBus
{
    private static readonly MethodInfo SendCommandMethod =
        typeof(OrleansBus).GetMethod(nameof(SendTypedCommand), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private static readonly MethodInfo RunQueryMethod =
        typeof(OrleansBus).GetMethod(nameof(RunTypedQuery), BindingFlags.NonPublic | BindingFlags.Instance)!;

    public Task SendCommand(IOrleansCommand command, GrainCancellationToken cancellationToken)
        => (Task)SendCommandMethod.MakeGenericMethod(command.GetType())
            .Invoke(this, [command, cancellationToken])!;

    private Task SendTypedCommand<TCommand>(TCommand command, GrainCancellationToken cancellationToken)
        where TCommand : IOrleansCommand
    {
        var handler = command.GrainType is not null
            ? grainFactory.GetGrain<IOrleansCommandHandler<TCommand>>(global::Orleans.Runtime.GrainId.Create(command.GrainType, command.GrainId))
            : grainFactory.GetGrain<IOrleansCommandHandler<TCommand>>(command.GrainId);
        return handler.Handle(command, cancellationToken);
    }

    public Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, GrainCancellationToken cancellationToken)
        where TResult : IQueryResult
        => (Task<TResult>)RunQueryMethod.MakeGenericMethod(query.GetType(), typeof(TResult))
            .Invoke(this, [query, cancellationToken])!;

    private Task<TResult> RunTypedQuery<TQuery, TResult>(TQuery query, GrainCancellationToken cancellationToken)
        where TQuery : IOrleansQuery<TResult>
        where TResult : IQueryResult
    {
        var handler = query.GrainType is not null
            ? grainFactory.GetGrain<IOrleansQueryHandler<TQuery, TResult>>(global::Orleans.Runtime.GrainId.Create(query.GrainType, query.GrainId))
            : grainFactory.GetGrain<IOrleansQueryHandler<TQuery, TResult>>(query.GrainId);
        return handler.Handle(query, cancellationToken);
    }

    public Task Publish(IOrleansEvent @event, CancellationToken cancellationToken = default)
    {
        var streamProvider = serviceProvider.GetRequiredKeyedService<IStreamProvider>(@event.StreamProvider);
        var stream = streamProvider.GetStream<IOrleansEvent>(@event.StreamId);
        return stream.OnNextAsync(@event);
    }
}
