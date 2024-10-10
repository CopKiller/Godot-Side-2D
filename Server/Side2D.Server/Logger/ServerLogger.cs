
using Core.Game.Interfaces.Logger;

namespace Side2D.Server.Logger
{
    internal class ServerLogger : ILogger
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
        public void PrintInfo(string message)
        {
            Console.WriteLine(message);
        }
        public void PrintWarning(string message)
        {
            Console.WriteLine(message);
        }
        public void PrintError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
