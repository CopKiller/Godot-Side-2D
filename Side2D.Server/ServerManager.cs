using Side2D.Server.Infrastructure;

namespace Side2D.Server;

public sealed class ServerManager
{
    private InitServer _InitServer { get; set; }
    
    public void InitServer()
    {
        _InitServer = new InitServer();
        _InitServer.Start();
    }
}