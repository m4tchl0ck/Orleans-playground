namespace Monolith.State;

public class GrainWithStateAsClass : Grain<StateAsClass>, IGrainWithStateAsClass
{
    public async Task<StateAsClass> GetState()
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
