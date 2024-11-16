using Core.Game.Interfaces.Service;
using Infrastructure.Logger;
using Side2D.Server.Logger;
using Side2D.Server.Services;

namespace Side2D.Server.Infrastructure;

internal class InitServer
{
    private ServerServicesManager? ServicesManager { get; set; }
    private bool IsRunning { get; set; }

    public void Start()
    {
        StartLogger();
        
        StartServices();
        
        Log.Print("Server Started...");

        IsRunning = true;
    }
    
    public void Stop()
    {
        if (!IsRunning) return;
        
        ServicesManager?.Manager.Stop();
        
        Log.Print("Server Stopped...");
        
        IsRunning = false;
    }
    
    public void Dispose()
    {
        ServicesManager?.Manager.Dispose();
    }
    
    private void StartLogger()
    {
        
        Log.LogInstance = new ServerLogger();
        Log.Print("Logs Initialized...");
    }
    
    private void StartServices()
    {
        // Initialize services manager.
        ServicesManager = new ServerServicesManager();
        
        // Register and start services.
        ServicesManager.Manager.Register();
        ServicesManager.Manager.Start();
        
        Log.Print("Services Initialized...");
        
        ServicesManager.Manager.Update();
    }
}