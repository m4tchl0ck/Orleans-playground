public class GrainWithStateAsStruct : Grain<StateAsStruct>, IGrainWithStateAsStruct
{
    public async Task<StateAsStruct> GetState()
    {
        await ReadStateAsync();
        return State;
    }

    public async Task Create()
    {
        State = new StateAsStruct("John Doe", 30);

        await WriteStateAsync();
    }
}
