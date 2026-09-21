using Orleans.Cqs;

[GenerateSerializer]
public record SetGreetingNameCommand(
    [property: Id(0)] string Id,
    [property: Id(1)] string Name) : IOrleansCommand
{
    string IOrleansCommand.GrainId => Id;
}
