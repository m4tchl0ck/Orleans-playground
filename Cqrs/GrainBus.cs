namespace Cqrs;

public class GrainBus(
    IClusterClient clusterClient
) : IBus
{
    public Task Send(ICommand command)
    {
        return Send((IGrainCommand)command);
    }

    private Task Send(IGrainCommand command)
    {
        var grainHandlerType = typeof(IGrainCommandHandler<>).MakeGenericType(command.GetType());
        var handler = clusterClient.GetGrain(grainHandlerType, command.GrainId);

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType()); 
        return (Task)handlerType.GetMethod("Handle")!.Invoke(handler, new object[] { command })!;
    }
}