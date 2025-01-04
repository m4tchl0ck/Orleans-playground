namespace Cqrs;

internal class GrainHandler(
    ISomething something
) : Grain,
    IGrainCommandHandler<GrainCommand1>,
    IGrainCommandHandler<GrainCommand2>
{
    public async Task Handle(GrainCommand1 command)
    {
        await something.Do(command.Name, command.Age);
    }

    public async Task Handle(GrainCommand2 command)
    {
        await something.Do(command.Name, command.Age);
    }
}