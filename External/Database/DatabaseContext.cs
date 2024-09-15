using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Side2D.Models;

namespace Database;

public sealed class DatabaseContext : DbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<PlayerModel> Players { get; set; }
    
    
    public DatabaseContext()
    {
        
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        // Configurar o sqlite
        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatabaseSide2D.db");
        optionsBuilder.UseSqlite($"Filename={databasePath}");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.AddInterceptors(new DetachEntitiesInterceptor());
    }

}