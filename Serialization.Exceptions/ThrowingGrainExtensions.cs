namespace Serialization.Exceptions;

public static class ThrowingGrainExtensions
{
    public static async Task WithExceptionHandling(this IThrowingGrain grain, Func<IThrowingGrain, Task> action)
    {
        try
        {
            await action(grain);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
