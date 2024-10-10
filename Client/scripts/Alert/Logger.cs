using Core.Game.Interfaces.Logger;
using Godot;

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