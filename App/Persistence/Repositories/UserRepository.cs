using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Persistence.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User, string>, IUserRepository
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
}