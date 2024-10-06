using Database;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Network;
using Side2D.Server.Database;
using Side2D.Server.Database.Interfaces;
using Side2D.Server.Database.Repositorys;
using Side2D.Server.Network;
using Side2D.Server.TempData;
using Side2D.Server.TempData.Interface;

namespace Side2D.Services;

public class Services
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureNetworkService(services);
        ConfigureDatabaseService(services);
        ConfigureTempDataService(services);
        
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
        services.AddScoped<IDatabaseService, DatabaseService>();
    }
    
    private void ConfigureTempDataService(IServiceCollection services)
    {
        services.AddSingleton<ITempDataService, TempDataService>();
    }
}