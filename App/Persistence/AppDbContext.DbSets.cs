using Domain;
using Infrastructure2.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
    {
        public required DbSet<Role> Roles { get; set; }
    }
}