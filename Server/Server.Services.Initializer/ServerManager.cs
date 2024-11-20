
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Core.Service.Logic;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Server.Service.Database;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Server.Services.Initializer;

public sealed class ServerManager
{
    public IServiceManager Manager { get; } 
    
    public ServerManager()
    {
        
        var serverServices = new ServerServices().GetServices();
        
        var serviceConfig = new ServiceConfiguration() { Enabled = true, NeedUpdate = true, UpdateIntervalMs = 1 };
        
        Manager = new ServiceManager(serviceConfig, serverServices);
        
        Manager.Register();
        Manager.Start();
    }
}