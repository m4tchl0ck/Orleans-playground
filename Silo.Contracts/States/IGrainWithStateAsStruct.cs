public interface IGrainWithStateAsStruct : ICreateable;


[GenerateSerializer]
public struct StateAsStruct(string name, int age) : ISnapshotable
{
    [Id(0)]
    public string Name { get; set; } = name;

    [Id(1)]
    public int Age { get; set; } = age;

    public Snapshot GetSnapshot()
        => new Snapshot(Name, Age);
}
