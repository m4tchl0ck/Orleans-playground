
public class ThrowingGrain : Grain, IThrowingGrain
{
    public Task ThrowException()
    {
        throw new Exception("Some exception");
    }
}