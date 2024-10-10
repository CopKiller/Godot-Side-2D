using Core.Game.Interfaces.Result;
using Core.Game.Models;

namespace Core.Game.Interfaces.Repositories;

public interface IAccountRepository : IRepository<AccountModel>
{
    Task<IException?> AddAccountAsync(AccountModel account);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<IResult<AccountModel?>> GetAccountAsync(string username, string password);
}
