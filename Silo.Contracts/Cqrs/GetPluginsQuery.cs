using Orleans.Cqs;

[GenerateSerializer]
public record GetPluginsQuery([property: Id(0)] string RequestId) : IOrleansQuery<PluginsResult>
{
    string IOrleansQuery.GrainId => RequestId;
}
