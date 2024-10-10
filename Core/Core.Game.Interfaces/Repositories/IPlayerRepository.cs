using Core.Game.Interfaces.Result;
using Core.Game.Models;

namespace Core.Game.Interfaces.Repositories;

public interface IPlayerRepository
{
    Task<IException?> AddPlayerAsync(int accountId, PlayerModel player);
    Task<IResult<List<PlayerModel>>> GetPlayersByAccountIdAsync(int accountId);
    Task<bool> NameExistsAsync(string username);
    Task<bool> UpdatePlayerAsync(PlayerModel player);
}