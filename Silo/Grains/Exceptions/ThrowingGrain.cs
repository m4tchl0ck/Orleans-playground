
public class ThrowingGrain : Grain, IThrowingGrain
{
    public Task ThrowException()
    {
        throw new Exception("Some exception");
    }

    public Task ThrowCustomException()
    {
        throw new CustomException("Some custom exception");
    }
}
