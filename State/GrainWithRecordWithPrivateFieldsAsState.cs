using Newtonsoft.Json;

namespace Monolith.State;

public interface IGrainWithRecordWithPrivateFieldsAsState : ICreateable<RecordWithPrivateFieldsAsState>;

public class GrainWithRecordWithPrivateFieldsAsState : Grain<RecordWithPrivateFieldsAsState>, IGrainWithRecordWithPrivateFieldsAsState
{
    public async Task<RecordWithPrivateFieldsAsState> GetState()
    {
        await ReadStateAsync();
        return State;
    }

    public async Task Create()
    {
        State.Create("John Doe", 30);

        await WriteStateAsync();
    }
}

[GenerateSerializer]
public record RecordWithPrivateFieldsAsState : IAgeable
{
    [Id(0)]
    public string Name { get; private set; }

    [Id(1), JsonProperty]private int _age;

    [JsonConstructor]
    private RecordWithPrivateFieldsAsState(string name, int age) => Create(name, age);

    public void Create(string name, int age)
    {
        Name = name;
        _age = age;
    }

    public int GetAge() => _age;
}
