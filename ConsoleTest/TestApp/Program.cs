

namespace TestApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var server = new Server.Services.Initializer.ServerManager();
        
        server.Manager.Register();
        server.Manager.Start();
    }
}