using Newtonsoft.Json;

public interface IGrainWithStateAsClassWithPrivateFields : ICreateable;

[GenerateSerializer]
public class StateAsClassWithPrivateFields : ISnapshotable
{
    [Id(0)]
    public string Name { get; private set; }

    [Id(1), JsonProperty]private int _age;

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private StateAsClassWithPrivateFields(string name, int age) => Create(name, age);
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void Create(string name, int age)
    {
        Name = name;
        _age = age;
    }

    public Snapshot GetSnapshot()
        => new Snapshot(Name, _age);
}