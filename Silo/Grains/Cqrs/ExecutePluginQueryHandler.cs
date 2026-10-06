using Microsoft.Extensions.Logging;
using Orleans.Cqs;

[global::Orleans.Concurrency.StatelessWorker]
internal class ExecutePluginQueryHandler(ILogger<ExecutePluginQueryHandler> logger)
    : Grain, IOrleansQueryHandler<ExecutePluginQuery, ExecutePluginResult>
{
    public async Task<ExecutePluginResult> Handle(ExecutePluginQuery query, GrainCancellationToken cancellationToken)
    {
        var kernel = GrainFactory.GetGrain<IKernelGrain>(0);
        var output = await kernel.Dispatch(query.Command, query.Args);
        logger.LogDebug("ExecutePluginQuery '{Command}' -> {Output}", query.Command, output);
        return new ExecutePluginResult(output);
    }
}
