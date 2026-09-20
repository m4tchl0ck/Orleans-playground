using Microsoft.Extensions.Logging;
using Orleans.Cqs;

[global::Orleans.Concurrency.StatelessWorker]
internal class GetPluginsQueryHandler(ILogger<GetPluginsQueryHandler> logger)
    : Grain, IOrleansQueryHandler<GetPluginsQuery, PluginsResult>
{
    public async Task<PluginsResult> Handle(GetPluginsQuery query, GrainCancellationToken cancellationToken)
    {
        var kernel = GrainFactory.GetGrain<IKernelGrain>(0);
        var plugins = await kernel.GetLoadedPlugins();
        logger.LogDebug("GetPluginsQuery returned {Count} plugins", plugins.Count);
        return new PluginsResult(plugins);
    }
}
