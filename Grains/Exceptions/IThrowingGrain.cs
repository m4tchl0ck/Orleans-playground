public interface IThrowingGrain : IGrainWithStringKey
{
    Task ThrowException();
    Task ThrowCustomException();
    Task ThrowCustomInheridException();
}

[GenerateSerializer]
public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}
