
namespace Server.Services.Initializer;

internal class ServerServices
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureDatabaseService(services);
        
        return services;
    }
    
    private void ConfigureDatabaseService(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
    }
}