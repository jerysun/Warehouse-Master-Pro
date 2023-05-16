using Core.Helpers;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.EF;

public class Repository<TId, T, TDbContext> : IRepository<TId, T>
where T : class
where TDbContext : notnull, DbContext
{
    private readonly TDbContext _dbContext;

    public Repository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id! }, cancellationToken);
    }

    public IQueryable<T> All()
    {
        return _dbContext.Set<T>();
    }

    public async Task<PagedList<T>> AllByPagesAsync(EntityParameters entityParameters)
    {
        return await PagedList<T>.ToPagedListAsync(All(),
            entityParameters.CurrentPage,
            entityParameters.PageSize);
    }

    public async Task<IList<T>> GetSomeAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().Where(expression).ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Entry<T>(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            throw new Exception($"The entity with id = '{id}' was not found.");
        }
        await DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(TId id)
          : base($"The entity with id = '{id}' was not found.") { }
    }
}

