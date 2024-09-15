using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Network;
using Side2D.Server.Database.Interfaces;
using Side2D.Server.Database.Repositorys;
using Side2D.Server.Network;
using Side2D.Server.Network.Interfaces;

namespace Side2D.Services;

public class Services
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureNetworkService(services);
        ConfigureDatabaseService(services);
        
        return services;
    }
    
    private void ConfigureNetworkService(IServiceCollection services)
    {
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddTransient<INetworkService, ServerNetworkService>();
    }
    
    private void ConfigureDatabaseService(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
    }
}