using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Service.Interfaces.Types;

namespace Core.Game.Interfaces.Repositories;

public interface IDatabaseService : ISingleService
{
    Task<List<IPlayerModel>?> GetPlayerModel(int accountId);
    Task<IAccountModel?> GetAccountModel(string username, string password);
}