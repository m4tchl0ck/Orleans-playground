namespace Orleans.Cqrs.Internal;

internal sealed class Bus(IOrleansBus orleansBus) : IBus
{
    public async Task SendCommand(IOrleansCommand command, CancellationToken cancellationToken = default)
    {
        var gcts = new GrainCancellationTokenSource();
        using var reg = cancellationToken.Register(static s => ((GrainCancellationTokenSource)s!).Cancel(), gcts);
        await orleansBus.SendCommand(command, gcts.Token);
    }

    public async Task<TResult> RunQuery<TResult>(IOrleansQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IQueryResult
    {
        var gcts = new GrainCancellationTokenSource();
        using var reg = cancellationToken.Register(static s => ((GrainCancellationTokenSource)s!).Cancel(), gcts);
        return await orleansBus.RunQuery(query, gcts.Token);
    }

    public Task Publish(IOrleansEvent @event, CancellationToken cancellationToken = default)
        => orleansBus.Publish(@event, cancellationToken);
}
