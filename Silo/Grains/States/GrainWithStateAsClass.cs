public class GrainWithStateAsClass : GrainWithState<StateAsClass>, IGrainWithStateAsClass
{
    public override async Task Create()
    {
        State.Name = "John Doe";
        State.Age = 30;

        await WriteStateAsync();
    }
}
