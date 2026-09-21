using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

[Command("throw-exception")]
public partial class ThrowExceptionCommand(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("grainId", 'g')]
    public string GrainId { get; set; } = "grain1-0";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IThrowingGrain>(GrainId);

        try
        {
            await grain.ThrowException();
        }
        catch (Exception e)
        {
            await console.Output.WriteLineAsync($"Exception type: {e.GetType().FullName}");
            await console.Output.WriteLineAsync($"Exception message: {e.Message}");
            await console.Output.WriteLineAsync($"Stack trace: {e.StackTrace}");
        }
    }
}