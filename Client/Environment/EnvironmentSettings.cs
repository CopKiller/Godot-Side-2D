using System;
using Godot;

namespace Side2D.Environment;

public static class EnvironmentSettings
{
    public static EnvironmentPlatforms CurrentPlatform => 
        (EnvironmentPlatforms)Enum.Parse(typeof(EnvironmentPlatforms), OS.GetName());
}