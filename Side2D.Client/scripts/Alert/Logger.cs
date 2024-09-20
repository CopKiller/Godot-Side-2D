using Godot;
using Side2D.Logger;

namespace Side2D.scripts.Alert;

public class Logger : ILogger
{
    public void Print(string message)
    {
        GD.Print(message);
    }

    public void PrintInfo(string message)
    {
        GD.Print(message);
    }

    public void PrintWarning(string message)
    {
        GD.Print(message);
    }

    public void PrintError(string message)
    {
        GD.PrintErr(message);
    }
}