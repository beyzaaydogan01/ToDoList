using Core.Entities;
using System.Linq.Expressions;

namespace Core.Repositories;
public interface IRepository<TEntiy, TId> where TEntiy : Entity<TId>, new()
{
    Task<List<TEntiy>> GetAllAsync(Expression<Func<TEntiy, bool>>? filter = null, bool enableAutoInclude = true);
    Task<TEntiy?> GetByIdAsync(TId id);
    Task<TEntiy?> UpdateAsync(TEntiy entity);
    Task<TEntiy?> AddAsync(TEntiy entity);
    Task<TEntiy?> RemoveAsync(TEntiy entity);
}