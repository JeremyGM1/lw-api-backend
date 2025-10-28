using Domain.Entities.Users;

namespace Application.Interfaces.Features.Users;

public interface IUserRepository : IGenericRepository<User, string>
{
    Task<User?> GetUserById(string Id);

    Task<List<User>> GetUsers();
}