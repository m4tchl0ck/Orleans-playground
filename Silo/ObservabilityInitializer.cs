using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;

public static class ObservabilityInitializer
{
    public static IHostBuilder UseObservability(this IHostBuilder hostBuilder)
        => hostBuilder
            .ConfigureLogging(logging => logging
                .AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.IncludeScopes = true;
                loggerOptions.IncludeFormattedMessage = true;
            }))
            .ConfigureServices(services =>
            {
                services.AddOpenTelemetry()
                    .ConfigureResource(options => options.AddService("MySilo"))
                    .WithMetrics(metrics =>
                    {
                        metrics
                            .AddRuntimeInstrumentation()
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddMeter("Microsoft.Orleans");
                    })
                    .WithTracing(traces =>
                    {
                        traces
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddSource("Microsoft.*")
                            .AddSource("Microsoft.Orleans.*")
                            .AddSource(ActivitySource.Name);
                    })
                    .WithLogging()
                    .UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf, new Uri("http://grafana:4318"));
            });
}