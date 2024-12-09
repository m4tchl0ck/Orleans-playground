public abstract class AbstractException : Exception
{
    public string ErrorCode { get; }

    public AbstractException(string errorCode, string message) 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}