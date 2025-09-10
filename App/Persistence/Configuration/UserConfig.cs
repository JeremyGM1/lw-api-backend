using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;


namespace Persistence.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id           = "00000000-0000-0000-0000-000000000000",
                FirstName    = "John",
                LastName     = "Smith",
                Email        = "John@gmail.com",
                Roles        = null,

                CreatedAt    = new DateTime(0001, 01, 01),
                UpdatedAt    = new DateTime(0001, 01, 01),
                CreatedBy    = "Seeder",
                UpdatedBy    = "Seeder"
            }
        );

        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).HasMaxLength(70).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).IsRequired();

        builder.Property(x => x.CreatedBy).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).HasMaxLength(50);
        builder.Property(x => x.UpdatedAt);
    }
}