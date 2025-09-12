using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Models.ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                 new ApplicationUser
                 {
                     Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                     Email = "superadmin@localhost.com",
                     NormalizedEmail = "SUPERADMIN@LOCALHOST.COM",
                     FirstName = "System",
                     LastName = "Super Administartor",
                     UserName = "superadmin@localhost.com",
                     NormalizedUserName = "SUPERADMIN@LOCALHOST.COM",
                     PasswordHash = "AQAAAAIAAYagAAAAEHTpnpv+i+I9mS6v4xd7q29DJEIU+H6DotE2xDvuqwll7wgpxdyjaI69QsYVga3qcw==", //the hashed password is P@ssword1
                     EmailConfirmed = true,
                     ConcurrencyStamp = "11111111-1111-1111-1111-111111111111", // <--- fijo
                     SecurityStamp = "22222222-2222-2222-2222-222222222222"  // <--- fijo
                 }
                 
            );
        }
    }
}
