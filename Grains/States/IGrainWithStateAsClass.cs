public interface IGrainWithStateAsClass : ICreateable<StateAsClass>;

[GenerateSerializer]
public class StateAsClass
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;

    [Id(1)]
    public int Age { get; set; }
}
