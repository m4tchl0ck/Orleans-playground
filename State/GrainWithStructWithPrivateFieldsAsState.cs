using Newtonsoft.Json;

namespace Monolith.State;

public interface IGrainWithStructWithPrivateFieldsAsState : ICreateable<StructWithPrivateFieldsAsState>;

public class GrainWithStructWithPrivateFieldsAsState : Grain<StructWithPrivateFieldsAsState>, IGrainWithStructWithPrivateFieldsAsState
{
    public async Task<StructWithPrivateFieldsAsState> GetState()
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
public struct StructWithPrivateFieldsAsState : IAgeable
{
    [Id(0)]
    public string Name { get; private set; }

    [Id(1), JsonProperty]private int _age;

    [JsonConstructor]
    private StructWithPrivateFieldsAsState(string name, int age) => Create(name, age);

    public void Create(string name, int age)
    {
        Name = name;
        _age = age;
    }

    public int GetAge() => _age;
}
