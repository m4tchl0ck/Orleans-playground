namespace Serialization.Exceptions;

public interface IThrowingGrain : IGrainWithStringKey
{
    Task ThrowException();
    Task ThrowCustomException();
}

public class ThrowingGrain : Grain, IThrowingGrain
{
    public Task ThrowException()
    {
        throw new Exception("Some exception");
    }

    public Task ThrowCustomException()
    {
        throw new CustomException("Some custom exception");
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}