public interface IThrowingGrain : IGrainWithStringKey
{
    Task ThrowException();
    Task ThrowCustomException();
}

[GenerateSerializer]
public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}
