﻿
using System.Diagnostics;
using Core.Game.Interfaces.Services.Network;

namespace Infrastructure.Network;

public class NetworkManager(NetworkService service)
{
    public bool NeedUpdate { get; set; } = true;
    public int DefaultUpdateInterval { get; set; } = 15;
    
    private NetworkService? NetworkService { get; set; } = service;
    public bool IsRunning;

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
