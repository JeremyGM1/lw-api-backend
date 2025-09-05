using Domain.AccessControl;

namespace Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserWithId(string Id);

    Task<List<User>> GetUsers();

    Task<bool> IsUnique(string id);
}