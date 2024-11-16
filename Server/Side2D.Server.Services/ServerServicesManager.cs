using Core.Game.Interfaces.Service;
using Core.Game.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Side2D.Server.Services;

public sealed class ServerServicesManager
{
    public IServicesManager Manager { get; } 
    
    public ServerServicesManager()
    {
        var serverServices = new ServerServices().GetServices();
        
        Manager = new ServicesManager(serverServices);
    }
}