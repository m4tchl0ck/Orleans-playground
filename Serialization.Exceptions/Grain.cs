namespace Serialization.Exceptions;

public interface IThrowingGrain : IGrainWithStringKey
{
    Task ThrowException();
}

public class ThrowingGrain : Grain, IThrowingGrain
{
    public Task ThrowException()
    {
        throw new Exception("Some exception");
    }
}
