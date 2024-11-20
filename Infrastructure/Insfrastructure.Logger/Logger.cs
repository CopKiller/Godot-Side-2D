using Microsoft.Extensions.Logging;
using ILogger = Core.Service.Interfaces.ILogger;

namespace Infrastructure.Logger;

public class Logger(string categoryName, LogLevel minLogLevel) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= minLogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        Console.WriteLine($"[{logLevel}] {categoryName}: {message}");
    }

    public void Print(string message)
    {
        this.LogInformation(new EventId(), message);
    }

    public void PrintInfo(string message)
    {
        this.LogInformation(new EventId(), message);
    }

    public void PrintWarning(string message)
    {
        this.LogInformation(new EventId(), message);
    }

    public void PrintError(string message)
    {
        this.LogInformation(new EventId(), message);
    }
}
