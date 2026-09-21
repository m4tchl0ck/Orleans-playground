using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;

[Command("throw-custom-inherit-exception")]
public partial class ThrowCustomInheritExceptionCommand(
    IClusterClient clusterClient) : ICommand
{
    [CommandOption("grainId", 'g')]
    public string GrainId { get; set; } = "grain1-0";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var grain = clusterClient.GetGrain<IThrowingGrain>(GrainId);

        try
        {
            await grain.ThrowCustomInheridException();
        }
        catch (CustomException e)
        {
            await console.Output.WriteLineAsync($"Exception type: {e.GetType().FullName}");
            await console.Output.WriteLineAsync($"Exception message: {e.Message}");
            await console.Output.WriteLineAsync($"Stack trace: {e.StackTrace}");
        }
    }
}