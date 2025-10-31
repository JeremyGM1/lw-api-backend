using Domain.Entities.Users;
using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DbContext
{
    public class TalentSuiteIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public TalentSuiteIdentityDbContext(
            DbContextOptions<TalentSuiteIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; } = default!;
        public DbSet<RolePermission> RolePermissions { get; set; } = default!;
        public DbSet<UserPermission> UserPermissions { get; set; } = default!;

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

            builder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(p => p.Description)
                      .HasMaxLength(300);
            });

            builder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(rp => rp.Id);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(rp => rp.RoleId)
                      .IsRequired();

                entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                      .IsUnique();
            });

            builder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("UserPermissions");
                entity.HasKey(up => up.Id);

                entity.HasOne(up => up.Permission)
                      .WithMany(p => p.UserPermissions)
                      .HasForeignKey(up => up.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(up => up.UserId)
                      .IsRequired();

                entity.Property(up => up.IsGranted)
                      .IsRequired();

                entity.HasIndex(up => new { up.UserId, up.PermissionId })
                      .IsUnique();
            });

        }
    }
}