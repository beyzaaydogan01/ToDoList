using Core.Entities;
using Core.Responses;

namespace Core.Services;

public interface IService<TEntity, TId, TResponse, TRequest, TUpdateRequest>
    where TEntity : Entity<TId>, new()
{
    Task<ReturnModel<TResponse>> AddAsync(TRequest create);
    Task<ReturnModel<TResponse>> UpdateAsync(TUpdateRequest update);
    Task<ReturnModel<TResponse>> GetByIdAsync(TId id);
    Task<ReturnModel<List<TResponse>>> GetAllAsync();
    Task<ReturnModel<TResponse>> DeleteAsync(TId id);
}