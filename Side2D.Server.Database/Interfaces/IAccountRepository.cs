using Database.Interfaces;
using Side2D.Models;
using Side2D.Server.Database.Results;

namespace Side2D.Server.Database.Interfaces;

public interface IAccountRepository : IRepository<AccountModel>
{
    Task<DatabaseException?> AddAccountAsync(AccountModel account);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<Result<AccountModel?>> GetAccountAsync(string username, string hashedPassword);
}
