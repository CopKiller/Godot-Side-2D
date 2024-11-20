using Core.Database.Interfaces;

namespace Core.Game.Interfaces.Repositories;

public interface IPlayerRepository<T> : IRepository<T> where T : class, IEntity
{
    Task<IDatabaseException?> AddPlayerAsync(int accountId, T player);
    Task<IResult<List<T>>> GetPlayersAsync(int accountId);
    Task<bool> NameExistsAsync(string username);
    Task<bool> UpdatePlayerAsync(T player);
}