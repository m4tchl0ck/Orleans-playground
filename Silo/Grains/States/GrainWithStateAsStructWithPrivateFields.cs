public class GrainWithStateAsStructWithPrivateFields : GrainWithState<StateAsStructWithPrivateFields>, IGrainWithStateAsStructWithPrivateFields
{
    public override async Task Create()
    {
        State.Create("John Doe", 30);

        await WriteStateAsync();
    }
}
