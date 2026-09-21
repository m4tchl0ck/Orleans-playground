using Microsoft.Extensions.DependencyInjection;
using Orleans.Cqs.Internal;

namespace Orleans.Cqs;

public static class OrleansCqsExtensions
{
    public static IServiceCollection AddOrleansCqs(this IServiceCollection services)
    {
        services.AddSingleton<IOrleansBus, OrleansBus>();
        return services;
    }

    public static IServiceCollection AddOrleansClusterCqs(this IServiceCollection services)
    {
        services.AddSingleton<IOrleansBus>(sp =>
            new OrleansBus(sp.GetRequiredService<IGrainFactory>()));
        return services;
    }
}
