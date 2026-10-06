public interface ICreateable : IGrainWithStringKey
{
    Task Create();
    Task<Snapshot> GetState();
}

public interface ISnapshotable
{
    Snapshot GetSnapshot();
}

[GenerateSerializer]
public record Snapshot(string Name, int Age);