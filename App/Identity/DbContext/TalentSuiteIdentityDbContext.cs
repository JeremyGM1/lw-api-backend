using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Identity.Models;

namespace Identity.DbContext
{
    public class TalentSuiteIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public TalentSuiteIdentityDbContext(
            DbContextOptions<TalentSuiteIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(TalentSuiteIdentityDbContext).Assembly);
            
            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}