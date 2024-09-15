using Database;
using Database.Repositorys;
using Microsoft.EntityFrameworkCore;
using Side2D.Models;
using Side2D.Server.Database.Interfaces;

namespace Side2D.Server.Database.Repositorys
{
    public class PlayerRepository(DatabaseContext context) : Repository<PlayerModel>(context), IPlayerRepository
    {
        public async Task<DatabaseException?> AddPlayerAsync(PlayerModel player)
        {
            if (await NameExistsAsync(player.Name))
                return new DatabaseException("Username already exists");
            
            await AddAsync(player);
            await SaveChangesAsync();
            
            return null;
        }
        

        public async Task<Result<List<PlayerModel>>> GetPlayersByAccountIdAsync(int accountId)
        {
            // Obtém os jogadores associados ao AccountId fornecido
            var players = await Context.Accounts
                .AsNoTracking()
                .Where(a => a.Id == accountId)
                .SelectMany(a => a.Players)
                .ToListAsync();


            return players.Any() ? new Result<List<PlayerModel>>(players, null) : new Result<List<PlayerModel>>([], new DatabaseException("You do not have characters!"));
        }
        
        public async Task<bool> NameExistsAsync(string name)
        {
            return await Context.Players.AsNoTracking().AnyAsync(p => p.Name == name);
        }
    }
}
