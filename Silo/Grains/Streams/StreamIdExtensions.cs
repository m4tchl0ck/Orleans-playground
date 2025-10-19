using System.Text;

public static class StreamIdExtensions
{
    public static bool IsNamespace(this StreamId streamId, string ns)
    {
        var nsBytes = Encoding.UTF8.GetBytes(ns);
        if (streamId.Namespace.Length != nsBytes.Length)
            return false;

        return streamId.Namespace.Span.SequenceEqual(nsBytes);
    }
}