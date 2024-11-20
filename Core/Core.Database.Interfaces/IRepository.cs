using System.Linq.Expressions;
using Core.Database.Interfaces.Account;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Database.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    /*protected Task<T?> GetByIdAsync(int id);
    protected Task<bool> ExistsAsync(string propertyName, object value);
    protected Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    protected Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 10);
    protected Task AddAsync(T entity);
    protected void Update(T entity);
    protected void Delete(T entity);
    protected Task<int> SaveChangesAsync();*/
}