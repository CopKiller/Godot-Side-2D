using Microsoft.Extensions.DependencyInjection;
using Side2D.Models;
using Side2D.Server.Database.Interfaces;

namespace Side2D.Server.Database;

public class DatabaseService(IAccountRepository accountRepository, 
                            IPlayerRepository playerRepository) : IDatabaseService
{

    public IAccountRepository AccountRepository => accountRepository;

    public IPlayerRepository PlayerRepository => playerRepository;

    public void Register() { }

    public void Start() { }

    public void Stop() { }

    public void Restart() { }

    public void Update(long currentTick) { }
    
    public void Dispose() { }
}