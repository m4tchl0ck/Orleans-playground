public interface IHelloWorld : IGrainWithStringKey
{
    Task<string> SayHello(string greeting);
    Task<IEnumerable<string>> GetGreetings();
}
