using Domain.AccessControl;

namespace Application.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserUnique(string FirstName);
}