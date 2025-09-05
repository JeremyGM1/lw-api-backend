using Domain.Entities;
using Infrastructure2.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(au => au.DomainUser)
                .WithOne()
                .HasForeignKey<ApplicationUser>(au => au.DomainUserId);
            builder.Entity<User>()
                .HasIndex(u => u.IdentityUserId)
                .IsUnique();

            builder.HasDefaultSchema("db_App_Intership");
        }
    }
}