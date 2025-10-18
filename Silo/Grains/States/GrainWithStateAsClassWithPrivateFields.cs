public class GrainWithStateAsClassWithPrivateFields : GrainWithState<StateAsClassWithPrivateFields>, IGrainWithStateAsClassWithPrivateFields
{
    public override async Task Create()
    {
        State.Create("John Doe", 30);

        await WriteStateAsync();
    }
}
