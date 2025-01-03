namespace Cqrs;

public record SimpleCommand1(
    string Name,
    int Age
) : ICommand;

internal class SimpleCommand1Handler(
    ISomething something
) : ICommandHandler<SimpleCommand1>
{
    public async Task Handle(SimpleCommand1 command)
    {
        await something.Do(command.Name, command.Age);
    }
}
