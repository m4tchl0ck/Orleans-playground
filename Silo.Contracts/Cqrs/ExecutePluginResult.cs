using Orleans.Cqrs;

[GenerateSerializer]
public record ExecutePluginResult(
    [property: Id(0)] string? Output) : IQueryResult;
