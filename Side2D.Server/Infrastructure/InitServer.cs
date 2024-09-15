
using Side2D.Logger;
using Side2D.Server.Logger;
using Side2D.Services;
using Side2D.Services.Configuration;

namespace Side2D.Server.Infrastructure;

internal class InitServer
{
    private ServicesManager? Services { get; set; }
    private bool _isRunning { get; set; }

    public void Start()
    {
        StartLogger();
        
        StartServices();
        
        Log.Print("Server Started...");

        _isRunning = true;
    }
    
    private void StartLogger()
    {
        Log.LogInstance = new ServerLogger();
        Log.Print("Logs Initialized...");
    }
    
    private void StartServices()
    {
        Services = new ServicesManager();
        Services.Register();
        Services.Start();
        
        Log.Print("Services Initialized...");
        
        Services.Update();
    }
}