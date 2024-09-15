using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Side2D.Models.Interfaces;

namespace Database.Repositorys;

public class Repository<T>(DatabaseContext context) : IRepository<T>
    where T : class, IEntity
{
    public readonly DatabaseContext Context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.AsNoTracking().FirstAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }
}
