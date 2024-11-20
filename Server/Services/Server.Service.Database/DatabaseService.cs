using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Game.Interfaces.Repositories;
using Core.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Service.Database;

public class DatabaseService(IServiceScopeFactory scopeFactory, ILogger logger) : IDatabaseService
{
    public IServiceConfiguration Configuration { get; } = new ServiceConfiguration() { Enabled = true, NeedUpdate = false, UpdateIntervalMs = 1000 };
    public void Register() { }

    public void Start()
    {
        logger.PrintInfo("Starting DatabaseService por dentro...");
    }
    public void Stop() { }
    
    public void Restart() { }

    public void Update(long tick) { }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() { }

    public async Task<List<IPlayerModel>?> GetPlayerModel(int accountId)
    {
        using var scope = scopeFactory.CreateScope();
        var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository<IPlayerModel>>();
        
        var result = await playerRepository.GetPlayersAsync(accountId);
        
        if (result.Error?.IsError == true)
        {
            // Log error
            
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