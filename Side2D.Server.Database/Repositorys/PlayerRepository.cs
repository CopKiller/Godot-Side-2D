using Database;
using Database.Repositorys;
using Microsoft.EntityFrameworkCore;
using Side2D.Models;
using Side2D.Server.Database.Interfaces;
using Side2D.Server.Database.Results;

namespace Side2D.Server.Database.Repositorys
{
    public delegate Task<bool> UpdatePlayerDelegate(PlayerModel playerModel);
    public class PlayerRepository(DatabaseContext context) : Repository<PlayerModel>(context), IPlayerRepository
    {
        public async Task<DatabaseException?> AddPlayerAsync(int accountId, PlayerModel player)
        {
            if (await NameExistsAsync(player.Name))
                return new DatabaseException("Username already exists");
            
            var account = await Context.Accounts
                .Include(a => a.Players)
                .FirstOrDefaultAsync(a => a.Id == accountId);
    
            if (account == null)
                return new DatabaseException("Account not found");
            
            account.Players.Add(player);
            Context.Accounts.Update(account);

            var result = await SaveChangesAsync();
            return result > 0 ? null : new DatabaseException("Failed to add player");
        }

        public async Task<Result<List<PlayerModel>>> GetPlayersByAccountIdAsync(int accountId)
        {
            // Obtém os jogadores associados ao AccountId fornecido
            var players = await Context.Accounts
                .AsNoTracking()
                .Where(a => a.Id == accountId)
                .SelectMany(a => a.Players)
                .ToListAsync();


            return players.Count != 0 ? new Result<List<PlayerModel>>(players, null) : new Result<List<PlayerModel>>([], new DatabaseException("You do not have characters!"));
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await Context.Players.AsNoTracking().AnyAsync(p => p.Name == name);
        }
        
        public async Task<bool> UpdatePlayerAsync(PlayerModel player)
        {
            Console.WriteLine($"Updating player {player.Id} {player.Name}");
            
            context.Players.Update(player);
            var result = await context.SaveChangesAsync() > 0;
            return result;
        }
    }
}
