using Domain.Common;

namespace Application.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<IReadOnlyList<TEntity>> GetAsync();

    Task<TEntity?> GetByIdAsync(TKey id);

    Task CreateAsync(TEntity Entity);

    Task UpdateAsync(TEntity Entity);

    Task DeleteAsync(TEntity Entity);
}