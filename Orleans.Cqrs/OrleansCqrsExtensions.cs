using Microsoft.Extensions.DependencyInjection;
using Orleans.Cqrs.Internal;

namespace Orleans.Cqrs;

public static class OrleansCqrsExtensions
{
    /// <summary>Register IBus for use inside a silo (uses IGrainFactory from the silo).</summary>
    public static IServiceCollection AddOrleansCqrs(this IServiceCollection services)
    {
        services.AddSingleton<IOrleansBus, OrleansBus>();
        services.AddSingleton<IBus, Bus>();
        return services;
    }

    /// <summary>Register IBus for use from a cluster client (gateway).</summary>
    public static IServiceCollection AddOrleansClusterCqrs(this IServiceCollection services)
    {
        services.AddSingleton<IOrleansBus>(sp =>
            new OrleansBus(sp.GetRequiredService<IClusterClient>(), sp));
        services.AddSingleton<IBus, Bus>();
        return services;
    }
}
