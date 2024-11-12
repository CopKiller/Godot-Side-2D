using Side2D.Server.Infrastructure;

namespace Side2D.Server;

public sealed class ServerManager
{
    private InitServer? InitServer { get; set; }
    
    public void Start()
    {
        InitServer = new InitServer();
        InitServer.Start();
    }
    
    public void Stop()
    {
        InitServer?.Stop();
    }
    
    public void Dispose()
    {
        InitServer?.Dispose();
    }
}