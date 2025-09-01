namespace Application.Interfaces.Persistence;

public interface IGenericRepository<T> where T : class // TODO Change class for BaseEntity
{
    Task<List<T>> GetAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> CreateAsync(T Entity);
    Task<T> UpdateAsync(T Entity);
    Task<T> DeleteAsync(T Entity);
}
