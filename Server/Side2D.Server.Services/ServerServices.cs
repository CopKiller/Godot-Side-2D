

using Core.Game.Interfaces.Network;
using Core.Game.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Network;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Server.Network;
using Side2D.Server.Repository.Repositorys;
using Side2D.Server.TempData;
using Side2D.Server.TempData.Interface;

namespace Side2D.Server.Services;

public class ServerServices : IServerServices
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
    }
    
    private void ConfigureTempDataService(IServiceCollection services)
    {
        services.AddSingleton<ITempDataService, TempDataService>();
    }
}