using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Game.Interfaces.Repositories;
using Core.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server.Service.Database;

public class DatabaseService(IServiceScopeFactory scopeFactory, ILogger<DatabaseService> log) : IDatabaseService
{
    public IServiceConfiguration Configuration { get; } = new DatabaseServiceConfiguration() { Enabled = true, NeedUpdate = true, UpdateIntervalMs = 1000 };
    public void Register() { }

    public void Start()
    {
        log.LogDebug("Starting DatabaseService Service...");
    }
    public void Stop() { }
    
    public void Restart() { }

    public void Update(long tick)
    {
        log.LogDebug("Updating DatabaseService por dentro...");
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        log.LogDebug("DatabaseService disposed.");
    }

    public IAccountRepository<IAccountModel> AccountRepository
    {
        get
        {
            using var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IAccountRepository<IAccountModel>>();
        }
    }
    
    public IPlayerRepository<IPlayerModel> PlayerRepository
    {
        get
        {
            using var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IPlayerRepository<IPlayerModel>>();
        }
    }

    public async Task<List<IPlayerModel>?> GetPlayerModel(int accountId)
    {
        using var scope = scopeFactory.CreateScope();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository<IPlayerModel>>();
        
        var result = await playerRepository.GetPlayersAsync(accountId);
        
        if (result.Error?.IsError == true)
        {
            
            return null;
        }

        // Realiza a operação
        return result.Value;
    }

    public async Task<IAccountModel?> GetAccountModel(string username, string password)
    {
        
        using var scope = scopeFactory.CreateScope();
        var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository<IAccountModel>>();
        
        var result = await accountRepository.GetAccountAsync(username, password);
        
        if (result.Error?.IsError == true)
        {
            // Log error
            
            return null;
        }

        // Realiza a operação
        return result.Value;
    }
}