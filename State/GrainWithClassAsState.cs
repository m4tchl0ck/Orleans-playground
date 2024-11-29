namespace Monolith.State;
public interface IGrainWithClassAsState : ICreateable<ClassAsState>;

public class GrainWithClassAsState : Grain<ClassAsState>, IGrainWithClassAsState
{
    public async Task<ClassAsState> GetState()
    {
        await ReadStateAsync();
        return State;
    }

    public async Task Create()
    {
        State.Name = "John Doe";
        State.Age = 30;

        await WriteStateAsync();
    }
}

[GenerateSerializer]
public class ClassAsState
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;

    [Id(1)]
    public int Age { get; set; }
}
