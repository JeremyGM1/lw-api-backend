using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Application.Interfaces.Features.Users;

namespace Persistence.Repositories;

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