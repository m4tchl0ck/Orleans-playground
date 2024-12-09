public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }
}

[GenerateSerializer]
public struct CustomExceptionTypeSurrogate
{
    [Id(0)]
    public string Message;
}

// This is a converter that converts between the surrogate and the foreign type.
[RegisterConverter]
public sealed class CustomExceptionTypeSurrogateConverter :
    IConverter<CustomException, CustomExceptionTypeSurrogate>
{
    public CustomException ConvertFromSurrogate(
        in CustomExceptionTypeSurrogate surrogate) =>
        new(surrogate.Message);

    public CustomExceptionTypeSurrogate ConvertToSurrogate(
        in CustomException value) =>
        new()
        {
            Message = value.Message
        };
}
