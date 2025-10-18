public interface IThrowingGrain : IGrainWithStringKey
{
    Task ThrowException();
}