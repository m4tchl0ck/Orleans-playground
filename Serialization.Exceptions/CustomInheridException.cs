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
    public string Message;
}

// This is a converter that converts between the surrogate and the foreign type.
[RegisterConverter]
public sealed class CustomInheridExceptionTypeSurrogateConverter :
    IConverter<CustomInheridException, CustomInheridExceptionTypeSurrogate>
{
    public CustomInheridException ConvertFromSurrogate(
        in CustomInheridExceptionTypeSurrogate surrogate) =>
        new(surrogate.Message);

    public CustomInheridExceptionTypeSurrogate ConvertToSurrogate(
        in CustomInheridException value) =>
        new()
        {
            Message = value.Message
        };
}