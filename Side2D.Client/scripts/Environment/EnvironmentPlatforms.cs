using System.Diagnostics.CodeAnalysis;

namespace Side2D.scripts.Environment;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum EnvironmentPlatforms
{
    None,
    Windows,
    macOS,
    Linux,
    FreeBSD,
    NetBSD,
    OpenBSD,
    BSD,
    iOS,
    Android,
    Web,
}