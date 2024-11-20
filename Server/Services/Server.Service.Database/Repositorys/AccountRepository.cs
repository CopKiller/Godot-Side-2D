
using Core.Database.Consistency;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Models;
using Core.Game.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Database.Repositorys;
using Microsoft.EntityFrameworkCore;
using Server.Service.Database.Results;

namespace Server.Service.Database.Repositorys;

public class AccountRepository<T>(DatabaseContext context) : Repository<T>(context), IAccountRepository<T>
where T : class, IAccountModel
{
    private DatabaseContext Context { get; } = context;
    public async Task<IDatabaseException?> AddAccountAsync(T account)
    {
        if (await ExistsAsync(p => p.Username == account.Username))
            return new DatabaseException("Account already exists");
        if (await ExistsAsync(p => p.Email == account.Email))
            return new DatabaseException("E-mail already exists");
        
        await AddAsync(account);
        
        var countChanges = await SaveChangesAsync();
        
        return countChanges > 0 ? null : new DatabaseException("Failed to add account");
    }
    
    public async Task<IResult<T>> GetAccountAsync(string username, string password)
    {
        var account = await Context.Accounts
                .Include(p => p.Players)
                .FirstOrDefaultAsync(p => p.Username == username && p.Password == password)
            as T;
        
        return account == null
            ? new Result<T>(account, new DatabaseException("Account not found")) 
            : new Result<T>(account, null);
    }
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await ExistsAsync(p => p.Email == email);
    }
    
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await ExistsAsync(p => p.Username == username);
    }
}
