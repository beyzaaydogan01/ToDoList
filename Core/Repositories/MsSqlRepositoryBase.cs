using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repositories;

public class MsSqlRepositoryBase<TContext, TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TContext : DbContext
{
    protected TContext Context { get; }

    public MsSqlRepositoryBase(TContext context)
    {
        Context = context;
    }

    public Task<TEntity?> AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, bool enableAutoInclude = true)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetByIdAsync(TId id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> RemoveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
