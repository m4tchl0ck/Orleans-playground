using Microsoft.Extensions.Logging;
using Orleans.Cqs;

internal class GreetingGrain(ILogger<GreetingGrain> logger)
    : Grain<GreetingState>,
      IOrleansCommandHandler<SetGreetingNameCommand>,
      IOrleansQueryHandler<GetGreetingQuery, GreetingResult>
{
    public async Task Handle(SetGreetingNameCommand command, GrainCancellationToken cancellationToken)
    {
        State.Name = command.Name;
        await WriteStateAsync();
        logger.LogInformation("Greeting name set to '{Name}' for id '{Id}'", command.Name, command.Id);
    }

    public Task<GreetingResult> Handle(GetGreetingQuery query, GrainCancellationToken cancellationToken)
    {
        var message = State.Name is not null
            ? $"Hello, {State.Name}!"
            : $"Hello, stranger! (no name set for '{query.Id}')";
        return Task.FromResult(new GreetingResult(message));
    }
}

[GenerateSerializer]
public record GreetingState
{
    [Id(0)] public string? Name { get; set; }
}
