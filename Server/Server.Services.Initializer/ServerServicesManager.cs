using Core.Game.Interfaces.Service;
using Core.Services.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Services.Initializer;

public sealed class ServerServicesManager
{
    public IServicesManager Manager { get; } 
    
    public ServerServicesManager()
    {
        var serverServices = new ServerServices().GetServices();
        
        Manager = new ServicesManager(serverServices);
    }
}