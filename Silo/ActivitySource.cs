using System.Runtime.CompilerServices;
using System.Diagnostics;

internal static class ActivitySource
{
    public const string Name = "my-silo";
    private static readonly System.Diagnostics.ActivitySource activitySource = new(Name);

    public static Activity? StartActivity([CallerMemberName] string name = "", ActivityKind kind = ActivityKind.Internal)
        => activitySource.StartActivity(name, kind);
}
