namespace MicroKernel.Core;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }
    IReadOnlyList<string> Commands { get; }

    string? Execute(string command, string[] args);
}
