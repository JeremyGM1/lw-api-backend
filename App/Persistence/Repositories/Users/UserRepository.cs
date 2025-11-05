using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Features.Users;
using Domain.Entities.Users;

namespace Persistence.Repositories.Users;

public class UserRepository(AppDbContext context) : GenericRepository<User, string>(context), IUserRepository
{
    public async Task<User?> GetUserById(string Id)
    {
        return await _context.Users.FindAsync(Id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
}