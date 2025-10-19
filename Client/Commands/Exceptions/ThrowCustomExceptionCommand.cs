using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

[Command("throw-custom-exception")]
public class ThrowCustomExceptionCommand(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("grainId", 'g')]
    public string GrainId { get; init; } = "grain1-0";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IThrowingGrain>(GrainId);

        try
        {
            await grain.ThrowCustomException();
        }
        catch (CustomException e)
        {
            await console.Output.WriteLineAsync($"Exception type: {e.GetType().FullName}");
            await console.Output.WriteLineAsync($"Exception message: {e.Message}");
            await console.Output.WriteLineAsync($"Stack trace: {e.StackTrace}");
        }
    }
}