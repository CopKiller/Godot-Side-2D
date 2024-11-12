
using Core.Game.Services;
using Infrastructure.Logger;
using Side2D.Server.Logger;
using Side2D.Server.Services;

namespace Side2D.Server.Infrastructure;

internal class InitServer
{
    private IServicesManager? ServicesManager { get; set; }
    private IServerServices? ServerServices { get; set; }
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
        
        ServicesManager?.Stop();
        
        Log.Print("Server Stopped...");
        
        IsRunning = false;
    }
    
    public void Dispose()
    {
        ServicesManager?.Dispose();
    }
    
    private void StartLogger()
    {
        
        Log.LogInstance = new ServerLogger();
        Log.Print("Logs Initialized...");
    }
    
    private void StartServices()
    {
        // Get my services collections.
        ServerServices = new ServerServices();
        
        // Initialize services manager.
        ServicesManager = new ServicesManager(ServerServices.GetServices());
        
        // Register and start services.
        ServicesManager.Register();
        ServicesManager.Start();
        
        Log.Print("Services Initialized...");
        
        ServicesManager.Update();
    }
}