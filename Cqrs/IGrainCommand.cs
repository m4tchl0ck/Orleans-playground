namespace Cqrs;

public interface IGrainCommand : ICommand
{
    string GrainId { get; }
}
