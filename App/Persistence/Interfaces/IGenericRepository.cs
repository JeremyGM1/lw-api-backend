using Domain.AccessControl;

namespace Persistence.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAsync();

    Task<T> GetByIdAsync(string id);

    Task CreateAsync(T Entity);

    Task UpdateAsync(T Entity);

    Task DeleteAsync(T Entity);

    Task<string> GetCreatedBy(string id);
}