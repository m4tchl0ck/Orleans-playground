public interface IGrainWithStateAsStruct : ICreateable<StateAsStruct>;


[GenerateSerializer]
public struct StateAsStruct(string name, int age)
{
    [Id(0)]
    public string Name { get; set; } = name;

    [Id(1)]
    public int Age { get; set; } = age;
}
