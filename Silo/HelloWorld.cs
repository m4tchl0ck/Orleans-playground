using Microsoft.Extensions.Logging;

public class HelloWorld(ILogger<HelloWorld> logger) : Grain<HelloWorldState>, IHelloWorld
{
    public Task<string> SayHello(string greeting)
    {
        logger.LogInformation("[[{grainId}] SayHello message received: greeting = '{greeting}'", this.GetPrimaryKeyString(), greeting);
        State.Grattings.Add(greeting);

        return Task.FromResult($"You said: '{greeting}', I say: Hello!");
    }

    public Task<IEnumerable<string>> GetGreetings()
    {
        logger.LogInformation("[[{grainId}] GetGreetings message received", this.GetPrimaryKeyString());
        return Task.FromResult(State.Grattings.AsEnumerable());
    }
}

public class HelloWorldState
{
    public List<string> Grattings { get; } = new List<string>();
}