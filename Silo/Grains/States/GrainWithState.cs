public abstract class GrainWithState<TState>: Grain<TState>
where TState : ISnapshotable
{
    public async Task<Snapshot> GetState()
    {
        await ReadStateAsync();
        return State.GetSnapshot();
    }

    public abstract Task Create();
}
