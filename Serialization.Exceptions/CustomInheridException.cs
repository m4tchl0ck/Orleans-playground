public class CustomInheridException : AbstractException
{
    public CustomInheridException(string message) 
        : base("CUSTOM_EXCEPTION", message)
    {
    }
}

[GenerateSerializer]
public struct CustomInheridExceptionTypeSurrogate
{
    [Id(0)]
    public readonly string ErrorMessage;

    public CustomInheridExceptionTypeSurrogate(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

[RegisterConverter]
public class CustomInheridExceptionConverter : AbstractExceptionSurrogateConverter<CustomInheridException>;

public abstract class AbstractExceptionSurrogateConverter<TAbstractException> :
    IConverter<TAbstractException, CustomInheridExceptionTypeSurrogate> where TAbstractException : AbstractException
{
    public TAbstractException ConvertFromSurrogate(
        in CustomInheridExceptionTypeSurrogate surrogate)
            => (TAbstractException)Activator.CreateInstance(type: typeof(TAbstractException), args: [surrogate.ErrorMessage])!;

    public CustomInheridExceptionTypeSurrogate ConvertToSurrogate(
        in TAbstractException value)
            => new(value.Message);
}