using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Side2D.Models.Interfaces;

namespace Database.Repositorys;

public class Repository<T>(DatabaseContext context) : IRepository<T>
    where T : class, IEntity
{
    protected readonly DatabaseContext Context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    protected async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.AsNoTracking().FirstAsync(x => x.Id == id);
    }

    protected async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    protected async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    protected void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    protected void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    protected async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}
