using Core.Helpers;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.IRepositories;

public interface IRepository<TId, T>
where T : class
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    IQueryable<T> All();
    Task<PagedList<T>> AllByPagesAsync(EntityParameters entityParameters);
    Task<IList<T>> GetSomeAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken);
}

