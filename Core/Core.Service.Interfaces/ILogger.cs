using Microsoft.Extensions.Logging.Abstractions;

namespace Core.Service.Interfaces;

public interface ILogger : Microsoft.Extensions.Logging.ILogger
{
    void Print(string message);
    void PrintInfo(string message);
    void PrintWarning(string message);
    void PrintError(string message);
}