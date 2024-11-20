using Core.Game.Interfaces.Services.Network;
using Infrastructure.Network;

namespace Side2D.Server.NetworkService;

public class ServerNetworkManager(INetworkService service) : INetworkManager
{
    public bool NeedUpdate { get; set; } = true;
    public int DefaultUpdateInterval { get; set; } = 15;
    public bool IsRunning;
    
    private INetworkService? NetworkService { get; set; } = service;
    
    public void Start()
    {
        NetworkService?.Start();
        
        IsRunning = true;
    }
    public void Register()
    {
        NetworkService?.Register();
    }
    public void Stop()
    {
        IsRunning = false;
        NetworkService?.Stop();
    }
    public void Restart()
    {
        Stop();
        Start();
    }
    public void Update(long currentTick)
    {
        if (!IsRunning) return;
        
        NetworkService?.Update(currentTick);
    }
    public void Dispose()
    {
        NetworkService?.Dispose();
    }

}
