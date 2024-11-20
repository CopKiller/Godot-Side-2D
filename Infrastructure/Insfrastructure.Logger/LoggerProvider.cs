using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class CustomLoggerProvider : ILoggerProvider
{
    private readonly LogLevel _minLogLevel;

    public CustomLoggerProvider(LogLevel minLogLevel)
    {
        _minLogLevel = minLogLevel;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName, _minLogLevel);
    }

    public void Dispose()
    {
        // Libera recursos
    }
}