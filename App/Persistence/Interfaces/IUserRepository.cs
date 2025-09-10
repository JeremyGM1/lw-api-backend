using Domain.Entities;

namespace Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task<User> GetUserWithId(string Id);

    Task<List<User>> GetUsers();
}