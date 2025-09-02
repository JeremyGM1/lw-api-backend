using Domain.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class AppDbContext : DbContext
    {
        public required DbSet<Role> Roles { get; set; }
    }
}