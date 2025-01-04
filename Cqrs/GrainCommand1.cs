namespace Cqrs;

[GenerateSerializer]
public record GrainCommand1(
    string Name,
    int Age
) : IGrainCommand
{
    string IGrainCommand.GrainId => Name;
}
