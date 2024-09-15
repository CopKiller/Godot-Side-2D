using Side2D.Models;

namespace Side2D.Server.Database.Interfaces;

public interface IPlayerRepository
{
    Task<DatabaseException?> AddPlayerAsync(PlayerModel player);
    Task<Result<List<PlayerModel>>> GetPlayersByAccountIdAsync(int accountId);
    Task<bool> NameExistsAsync(string username);
}