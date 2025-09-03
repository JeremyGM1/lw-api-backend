using Microsoft.EntityFrameworkCore;

using Application.Interfaces.Persistence;
using Domain.AccessControl;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> IsUserUnique(string firstName)
    {
        return _context.Users.AnyAsync(x => x.FirstName == firstName);
    }

    public async Task<User> GetUserWithId(string Id)
    {
        return await _context.Users.FindAsync(Id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}