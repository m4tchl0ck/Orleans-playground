namespace Monolith.State;

public interface ICreateable<TState> : IGrainWithStringKey
{
    Task Create();
    Task<TState> GetState();
}
