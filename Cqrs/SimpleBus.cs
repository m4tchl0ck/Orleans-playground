namespace Cqrs;

public class SimpleBus(
    IServiceProvider serviceProvider
) : IBus
{
    public Task Send(ICommand command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for {command.GetType().Name} not found");
        }
        var method = handler.GetType().GetMethod("Handle")!;
        return (Task)method.Invoke(handler, new object[] { command })!;
    }
}
