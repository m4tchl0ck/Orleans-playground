using Newtonsoft.Json;

namespace Monolith.State;

public interface IGrainWithClassWithPrivateFieldsAsState : ICreateable<ClassWithPrivateFieldsAsState>;

public class GrainWithClassWithPrivateFieldsAsState : Grain<ClassWithPrivateFieldsAsState>, IGrainWithClassWithPrivateFieldsAsState
{
    public async Task<ClassWithPrivateFieldsAsState> GetState()
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
public class ClassWithPrivateFieldsAsState : IAgeable
{
    [Id(0)]
    public string Name { get; private set; }

    [Id(1), JsonProperty]private int _age;
    
    [JsonConstructor]
    private ClassWithPrivateFieldsAsState(string name, int age) => Create(name, age);

    public void Create(string name, int age)
    {
        Name = name;
        _age = age;
    }

    public int GetAge() => _age;
}
