using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Domain.AccessControl;

namespace Persistence.Configuration;
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id           = Guid.Empty.ToString(),
                FirstName    = "John",
                LastName     = "Smith",
                Email        = "John@gmail.com",
                PasswordHash = ""
            }
        );

        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).HasMaxLength(70).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();

        builder.Property(x => x.CreatedBy).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).HasMaxLength(50).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

    }
}
