[GenerateSerializer]
public record PluginDescriptor(
    [property: Id(0)] string Name,
    [property: Id(1)] string Description,
    [property: Id(2)] IReadOnlyList<string> Commands);
