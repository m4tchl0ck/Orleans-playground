namespace Cqrs;

[GenerateSerializer]
public record GrainCommand2(
    string Name,
    int Age
) : IGrainCommand
{
    string IGrainCommand.GrainId => Name;
}