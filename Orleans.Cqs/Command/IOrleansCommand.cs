namespace Orleans.Cqs;

public interface IOrleansCommand
{
    string GrainId { get; }
    string? GrainType => null;
}
