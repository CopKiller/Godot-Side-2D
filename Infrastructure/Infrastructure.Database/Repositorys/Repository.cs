using System.Linq.Expressions;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database.Repositorys;

public class Repository<T>(DatabaseContext context) : IRepository<T>
    where T : class, IEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    protected async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    protected async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    
    protected async Task<bool> ExistsAsync(string propertyName, object value)
    {
        return await _dbSet
            .AnyAsync(entity => EF.Property<object>(entity, propertyName) == value);
    }

    protected async Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
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
        return await context.SaveChangesAsync();
    }
}
