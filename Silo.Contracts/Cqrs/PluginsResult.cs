using Orleans.Cqs;

[GenerateSerializer]
public record PluginsResult(
    [property: Id(0)] IReadOnlyList<PluginDescriptor> Plugins) : IQueryResult;
