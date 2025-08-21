using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
    }
}