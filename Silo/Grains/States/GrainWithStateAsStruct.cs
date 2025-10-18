public class GrainWithStateAsStruct : GrainWithState<StateAsStruct>, IGrainWithStateAsStruct
{
    public override async Task Create()
    {
        State = new StateAsStruct("John Doe", 30);

        await WriteStateAsync();
    }
}
