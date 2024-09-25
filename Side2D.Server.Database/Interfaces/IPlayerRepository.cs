using Side2D.Models;
using Side2D.Models.Interfaces;
using Side2D.Server.Database.Results;

namespace Side2D.Server.Database.Interfaces;

public interface IPlayerRepository
{
    Task<DatabaseException?> AddPlayerAsync(int accountId, PlayerModel player);
    Task<Result<List<PlayerModel>>> GetPlayersByAccountIdAsync(int accountId);
    Task<bool> NameExistsAsync(string username);
}