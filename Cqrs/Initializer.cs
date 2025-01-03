using Microsoft.Extensions.DependencyInjection;

namespace Cqrs;

public static class Initializer
{
    public static void InitializeCqrs(this IServiceCollection services)
    {
        services
            .AddSingleton<IBus, SimpleBus>();

        services
            .AddTransient<ICommandHandler<SimpleCommand1>, SimpleCommand1Handler>();
    }
}