using Domain.AccessControl;

namespace Application.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserUnique(string FirstName);

    Task<User> GetUserWithId(string Id);

    Task<List<User>> GetUsers();
}