using Orleans.Cqrs;

[GenerateSerializer]
public record PluginsResult(
    [property: Id(0)] IReadOnlyList<PluginDescriptor> Plugins) : IQueryResult;
