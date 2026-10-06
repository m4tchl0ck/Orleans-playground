using Newtonsoft.Json;

public interface IGrainWithStateAsRecordWithPrivateFields : ICreateable;

[GenerateSerializer]
public record StateAsRecordWithPrivateFields : ISnapshotable
{
    [Id(0)]
    public string Name { get; private set; }

    [Id(1), JsonProperty] private int _age;

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private StateAsRecordWithPrivateFields(string name, int age) => Create(name, age);
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void Create(string name, int age)
    {
        Name = name;
        _age = age;
    }

    public Snapshot GetSnapshot()
        => new Snapshot(Name, _age);
}
