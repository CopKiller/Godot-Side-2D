using Microsoft.Extensions.DependencyInjection;
using Side2D.Network;
using Side2D.Server.Network;

namespace Side2D.Services;

public class Services
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureNetworkService(services);
        
        return services;
    }
    
    private void ConfigureNetworkService(IServiceCollection services)
    {
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddTransient<INetworkService, ServerNetworkService>();
    }
}