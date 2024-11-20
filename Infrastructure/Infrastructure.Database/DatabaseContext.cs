using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    
    public DbSet<PlayerModel> Players { get; set; }
    
    public DatabaseContext()
    {
        //Database.Migrate();
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        //Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Adiciona índice para Email
        modelBuilder.Entity<AccountModel>()
            .HasIndex(a => a.Email)
            .IsUnique();

        // Adiciona índice para Username
        modelBuilder.Entity<AccountModel>()
            .HasIndex(a => a.Username)
            .IsUnique();
        
        // Adiciona índice para Name
        modelBuilder.Entity<PlayerModel>()
            .HasIndex(a => a.Name)
            .IsUnique();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatabaseSide2D.db");
        optionsBuilder.UseSqlite($"Filename={databasePath}");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.AddInterceptors(new DetachEntitiesInterceptor());
    }
}