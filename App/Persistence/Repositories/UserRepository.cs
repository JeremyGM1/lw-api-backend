using Microsoft.EntityFrameworkCore;

using Domain.AccessControl;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<User> GetUserWithId(string Id)
    {
        return await _context.Users.FindAsync(Id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> IsUnique(string id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null;
    }
}