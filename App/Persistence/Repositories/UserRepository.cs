using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User, string>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserById(string Id)
    {
        return await _context.Users.FindAsync(Id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}