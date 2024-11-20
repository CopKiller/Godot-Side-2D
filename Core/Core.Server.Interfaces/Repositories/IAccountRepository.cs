using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;

namespace Core.Game.Interfaces.Repositories;

public interface IAccountRepository<T> : IRepository<T> where T : class, IAccountModel
{
    Task<IDatabaseException?> AddAccountAsync(T account);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<IResult<T>> GetAccountAsync(string username, string password);
}
