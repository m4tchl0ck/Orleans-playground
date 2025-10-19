public interface IGrainWithStateAsClass : ICreateable;

[GenerateSerializer]
public class StateAsClass : ISnapshotable
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;

    [Id(1)]
    public int Age { get; set; }

    public Snapshot GetSnapshot()
        => new Snapshot(Name, Age);
}
