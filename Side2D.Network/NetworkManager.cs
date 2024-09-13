
using System.Diagnostics;

namespace Side2D.Network;

public class NetworkManager : INetworkManager
{
    private INetworkService? NetworkService { get; set; }
    public bool _isRunning;
    
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
        
        _isRunning = true;
    }
    
    public void Register()
    {
        NetworkService?.Register();
    }
    public void Stop()
    {
        _isRunning = false;
        NetworkService?.Stop();
    }

    public void Restart()
    {
        Stop();
        Start();
    }

    public void Update()
    {
        if (!_isRunning) return;
        
        if (DefaultUpdateInterval > 0) 
            if (_stopwatch.ElapsedMilliseconds < DefaultUpdateInterval) return;
        
        NetworkService?.Update();
        
        if (DefaultUpdateInterval > 0) 
            _stopwatch.Restart();
    }

    public void Dispose()
    {
        Stop();
        NetworkService?.Dispose();
    }

}
