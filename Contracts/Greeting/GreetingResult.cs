using Orleans.Cqs;

[GenerateSerializer]
public record GreetingResult(
    [property: Id(0)] string Message) : IQueryResult;
