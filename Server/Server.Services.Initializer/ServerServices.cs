using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Game.Interfaces.Repositories;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Core.Service.Logic;
using Infrastructure.Database;
using Infrastructure.Logger;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Service.Database.Repositorys;
using Server.Service.Database;
using ILogger = Core.Service.Interfaces.ILogger;

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
        // Adiciona o logging do Microsoft.Extensions.Logging
        services.AddLogging(loggingBuilder =>
        {
            // Adiciona o provider customizado
            loggingBuilder.AddProvider(new CustomLoggerProvider(LogLevel.Information));
        });

        // Registro do Logger customizado (associado à sua interface `ILogger`)
        services.AddScoped<ILogger>(serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return new Logger("Server", LogLevel.Information);
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