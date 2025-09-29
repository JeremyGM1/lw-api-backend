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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(TalentSuiteIdentityDbContext).Assembly);
        }
    }
}