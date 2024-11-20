
using Core.Database.Consistency;
using Core.Database.Interfaces;
using Core.Database.Models;
using Core.Game.Interfaces.Repositories;
using Infrastructure.Database;
using Infrastructure.Database.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Server.Service.Database.Results;

namespace Server.Service.Database.Repositorys
{
    public class PlayerRepository<T>(DatabaseContext context) : Repository<T>(context), IPlayerRepository<T>
        where T : class, IPlayerModel
    {
        private DatabaseContext Context { get; } = context;
        
        public async Task<IDatabaseException?> AddPlayerAsync(int accountId, T player)
        {
            if (await ExistsAsync(p => p.Name == player.Name))
                return new DatabaseException("Player already exists");
            
            var account = await Context.Accounts.FirstAsync(p => p.Id == accountId);
            
            account.Players.Add(player);
            
            await AddAsync(player);
            
            var countChanges = await SaveChangesAsync();
            
            return countChanges > 0 ? null : new DatabaseException("Failed to add player");
        }
        
        public async Task<IResult<List<T>>> GetPlayersAsync(int accountId)
        {
            var players = await Context.Players
                .Include(p => p.Position)
                .Include(p => p.Vitals)
                .Include(p => p.Stats)
                .Where(p => p.AccountModelId == accountId)
                .ToListAsync() as List<T>;

            // Encapsular o resultado em um IResult
            return players?.Count == 0 ? new Result<List<T>>(players, new DatabaseException("Players not found")) : new Result<List<T>>(players, null);
        }

        public Task<bool> NameExistsAsync(string username)
        {
            return ExistsAsync(p => p.Name == username);
        }

        public async Task<bool> UpdatePlayerAsync(T player)
        {
            Console.WriteLine($"Updating player {player.Id} {player.Name}...");

            Update(player);

            var result = await SaveChangesAsync() > 0;
            return result;
        }
    }
}
