

using Core.Game.Interfaces.Logger;

namespace Infrastructure.Logger
{
    public sealed class Log
    {
        public static ILogger? LogInstance { get; set; }
        
        public Log(ILogger? logger)
        {
            LogInstance = logger;
        }

        private static string GetTimestamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss");
        }

        public static void Print(string message)
        {
            LogInstance?.Print($"{GetTimestamp()}: {message}");
        }

        public static void PrintInfo(string message)
        {
            LogInstance?.Print($"[INFO]{GetTimestamp()}: {message}");
        }

        public static void PrintWarning(string message)
        {
            LogInstance?.PrintError($"[WARNING]{GetTimestamp()}: {message}");
        }

        public static void PrintError(string message)
        {
            LogInstance?.PrintError($"[ERROR]{GetTimestamp()}: {message}");
        }
    }
}
