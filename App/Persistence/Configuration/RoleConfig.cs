using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class RoleConfig
    {
        public RoleConfig(EntityTypeBuilder<Role> entityBuilder)
        {
            entityBuilder.ToTable("role", schema: "application");
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).HasMaxLength(20).IsRequired();
            entityBuilder.Property(x => x.Description).HasMaxLength(100).IsRequired();

        entityBuilder.Property(x => x.CreatedBy).HasMaxLength(50).IsRequired();
        entityBuilder.Property(x => x.CreatedAt).IsRequired();
        entityBuilder.Property(x => x.UpdatedBy).HasMaxLength(50).IsRequired();
        entityBuilder.Property(x => x.UpdatedAt).IsRequired();
    }
}