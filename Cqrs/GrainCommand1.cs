namespace Cqrs;

[GenerateSerializer]
public record GrainCommand1(
    string Name,
    int Age
) : IGrainCommand
{
    string IGrainCommand.GrainId => Name;
}

internal class GrainHandler(
    ISomething something
) : Grain,
    IGrainCommandHandler<GrainCommand1>
{
    public async Task Handle(GrainCommand1 command)
    {
        await something.Do(command.Name, command.Age);
    }
}