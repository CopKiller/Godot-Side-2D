using Database;
using Database.Repositorys;
using Microsoft.EntityFrameworkCore;
using Side2D.Cryptography;
using Side2D.Models;
using Side2D.Server.Database.Interfaces;
using Side2D.Server.Database.Results;

namespace Side2D.Server.Database.Repositorys;

public class AccountRepository(DatabaseContext context) : Repository<AccountModel>(context), IAccountRepository
{
    public async Task<DatabaseException?> AddAccountAsync(AccountModel account)
    {
        if (await EmailExistsAsync(account.Email))
            return new DatabaseException("Email already exists");
        if (await UsernameExistsAsync(account.Username))
            return new DatabaseException("Username already exists");

        account.Password = PasswordHelper.HashPassword(account.Password);

        await AddAsync(account);
        
        var countChanges = await SaveChangesAsync();
        
        return countChanges > 0 ? null : new DatabaseException("Failed to add account");
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Context.Accounts.AsNoTracking().AnyAsync(a => a.Email == email);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await Context.Accounts.AsNoTracking().AnyAsync(a => a.Username == username);
    }

    public async Task<Result<AccountModel?>> GetAccountAsync(string username, string password)
    {
        
        var account = await Context.Accounts.AsNoTracking()
            .Include(a => a.Players)
            .ThenInclude(a => a.Position)
            .Include(a => a.Players)
            .ThenInclude(a => a.Vitals)
            .Include(a => a.Players)
            .ThenInclude(a => a.Attributes)
            .FirstOrDefaultAsync(a => a.Username == username);
        
        if (account != null && !PasswordHelper.VerifyPassword(password, account.Password))
            return new Result<AccountModel?>(null, new DatabaseException("Invalid username or password"));
        
        return new Result<AccountModel?>(account, null);
    }
}
