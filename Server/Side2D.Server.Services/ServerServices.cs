

using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Network;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Repositories;
using Core.Game.Interfaces.TempData;
using Infrastructure.Database;
using Infrastructure.Network;
using Microsoft.Extensions.DependencyInjection;
using Side2D.Server.Attributes;
using Side2D.Server.Combat;
using Side2D.Server.Network;
using Side2D.Server.Physics;
using Side2D.Server.Repository.Repositorys;
using Side2D.Server.TempData;

namespace Side2D.Server.Services;

public class ServerServices : IServerServices
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureNetworkService(services);
        ConfigureDatabaseService(services);
        ConfigureTempDataService(services);
        ConfigurePhysicService(services);
        ConfigureAttributesService(services);
        ConfigureCombatService(services);
        
        return services;
    }
    
    private void ConfigureNetworkService(IServiceCollection services)
    {
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddScoped<INetworkService, ServerNetworkService>();
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
    
    private void ConfigurePhysicService(IServiceCollection services)
    {
        services.AddSingleton<IPhysicService, PhysicService>();
    }
    
    private void ConfigureAttributesService(IServiceCollection services)
    {
        services.AddSingleton<IAttributeService, AttributeService>();
    }
    
    private void ConfigureCombatService(IServiceCollection services)
    {
        services.AddSingleton<ICombatService, CombatService>();
    }
}