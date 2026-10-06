using Orleans.Cqs;

[GenerateSerializer]
public record GetGreetingQuery(
    [property: Id(0)] string Id) : IOrleansQuery<GreetingResult>
{
    string IOrleansQuery.GrainId => Id;
}
