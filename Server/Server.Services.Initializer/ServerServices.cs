using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Game.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Service.Database.Repositorys;
using Server.Service.Database;

namespace Server.Services.Initializer;

internal class ServerServices
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();
        
        ConfigureLoggerService(services);
        ConfigureDatabaseService(services);
        
        return services;
    }

    private void ConfigureLoggerService(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Debug;
        
        // Adiciona o logging do Microsoft.Extensions.Logging
        services.AddLogging(loggingBuilder =>
        {
            // Configura o nível mínimo para todos os logs
            loggingBuilder.SetMinimumLevel(logLevel);
            
            // Adiciona o provider customizado
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });
    }
    
    private void ConfigureDatabaseService(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();
        services.AddScoped<IAccountRepository<IAccountModel>, AccountRepository<IAccountModel>>();
        services.AddScoped<IPlayerRepository<IPlayerModel>, PlayerRepository<IPlayerModel>>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
    }
}