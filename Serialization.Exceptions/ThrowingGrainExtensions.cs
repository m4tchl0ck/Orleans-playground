using Microsoft.Extensions.Logging;

namespace Serialization.Exceptions;

public static class ThrowingGrainExtensions
{
    public static async Task WithExceptionHandling(
        this IThrowingGrain grain, 
        ILogger logger, Func<IThrowingGrain, Task> action)
    {
        try
        {
            await action(grain);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception occured");
        }
    }
}
