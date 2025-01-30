using Microsoft.Extensions.Logging;

public class HelloWorld(ILogger<HelloWorld> logger) : Grain, IHelloWorld
{
    public Task<string> SayHello(string greeting)
    {
        logger.LogInformation("[[{grainId}] SayHello message received: greeting = '{greeting}'", this.GetPrimaryKeyString(), greeting);
        return Task.FromResult($"You said: '{greeting}', I say: Hello!");
    }
}
