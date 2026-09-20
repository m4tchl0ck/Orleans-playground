namespace Orleans.Cqrs;

public interface IOrleansCommand : ICommand
{
    string GrainId { get; }
    string? GrainType => null;
}
