namespace Serialization.Exceptions;

public class CustomInheridException : AbstractException
{
    public CustomInheridException(string message) 
        : base("CUSTOM_EXCEPTION", message)
    {
    }
}