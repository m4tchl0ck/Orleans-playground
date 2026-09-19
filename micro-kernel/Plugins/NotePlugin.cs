using MicroKernel.Core;

namespace MicroKernel.Plugins;

// Shows that plugins can be stateful — state lives entirely inside the plugin,
// invisible to the kernel.
public sealed class NotePlugin : IPlugin
{
    private readonly List<string> _notes = [];

    public string Name => "Notepad";
    public string Description => "Stateful notepad: note <text> | notes | clearnotes";
    public IReadOnlyList<string> Commands => ["note", "notes", "clearnotes"];

    public string? Execute(string command, string[] args) => command switch
    {
        "note" when args.Length == 0 => "Usage: note <text>",
        "note" => AddNote(string.Join(" ", args)),
        "notes" => _notes.Count == 0
            ? "(no notes)"
            : string.Join("\n", _notes.Select((n, i) => $"  {i + 1}. {n}")),
        "clearnotes" => ClearNotes(),
        _ => null
    };

    private string AddNote(string text)
    {
        _notes.Add(text);
        return $"Note #{_notes.Count} saved.";
    }

    private string ClearNotes()
    {
        var count = _notes.Count;
        _notes.Clear();
        return $"Cleared {count} note(s).";
    }
}
