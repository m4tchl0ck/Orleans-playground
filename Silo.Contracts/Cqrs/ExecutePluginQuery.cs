using Orleans.Cqrs;

[GenerateSerializer]
public record ExecutePluginQuery(
    [property: Id(0)] string Command,
    [property: Id(1)] string[] Args) : IOrleansQuery<ExecutePluginResult>
{
    string IOrleansQuery.GrainId => Command;
}
