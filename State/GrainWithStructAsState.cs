namespace Monolith.State;

public interface IGrainWithStructAsState : ICreateable<StructAsState>;

public class GrainWithStructAsState : Grain<StructAsState>, IGrainWithStructAsState
{
    public async Task<StructAsState> GetState()
    {
        await ReadStateAsync();
        return State;
    }

    public async Task Create()
    {
        State = new StructAsState("John Doe", 30);

        await WriteStateAsync();
    }
}

[GenerateSerializer]
public struct StructAsState(string name, int age)
{
    [Id(0)]
    public string Name { get; set; } = name;

    [Id(1)]
    public int Age { get; set; } = age;
}
