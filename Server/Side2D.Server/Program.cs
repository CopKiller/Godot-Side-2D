
using Side2D.Server.Infrastructure;

namespace Side2D.Server;

public class Program
{
    private static ServerManager? _serverManager;
    public static void Main(string[] args)
    {
        _serverManager = new ServerManager();
        _serverManager.Start();
    }
}