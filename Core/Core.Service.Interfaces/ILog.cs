using Microsoft.Extensions.Logging;

namespace Core.Service.Interfaces;

public interface ILog : ILogger
{
    void Print(string message, params object[] args);
    void PrintInfo(string message, params object[] args);
    void PrintWarning(string message, params object[] args);
    void PrintError(string message, params object[] args);
}