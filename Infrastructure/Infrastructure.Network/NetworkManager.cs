
using System.Diagnostics;
using Core.Game.Interfaces.Services.Network;

namespace Infrastructure.Network;

public class NetworkManager : INetworkManager
{
    private INetworkService? NetworkService { get; set; }
    public bool IsRunning;
    
    public int DefaultUpdateInterval = 15;
    private readonly Stopwatch _stopwatch = new();
    
    public NetworkManager(INetworkService service)
    {
        NetworkService = service;
    }

    public void Start()
    {
        NetworkService?.Start();
        
        if (DefaultUpdateInterval > 0) 
            _stopwatch.Start();
        
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
        
        if (DefaultUpdateInterval > 0) 
            if (_stopwatch.ElapsedMilliseconds < DefaultUpdateInterval) return;
        
        NetworkService?.Update(currentTick);
        
        if (DefaultUpdateInterval > 0) 
            _stopwatch.Restart();
    }

    public void Dispose()
    {
        Stop();
        NetworkService?.Dispose();
    }

}
