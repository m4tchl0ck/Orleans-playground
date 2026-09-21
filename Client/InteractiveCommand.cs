using CliFx;
using CliFx.Binding;
using CliFx.Infrastructure;
using Microsoft.Extensions.Logging;

[Command]
public partial class InteractiveCommand(
    ILogger<InteractiveCommand> logger,
    CommandLineApplication cliApplication
    ) : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.Output.WriteLine("Interactive CLI started. Type 'exit' to quit.");

        while (true)
        {
            console.Output.Write("> ");
            string? input = console.Input.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Trim().ToLower() == "exit")
                break;

            try
            {
                await cliApplication.RunAsync(input.Split(' '));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Command execution failed");
                console.Output.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
