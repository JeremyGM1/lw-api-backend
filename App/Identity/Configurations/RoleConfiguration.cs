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
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                    Name = "Super Administrator",
                    NormalizedName = "SUPER ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "5d2f8b9c-8f61-4a40-94b8-6b27b5b7e0d3",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "1a7e92c4-2e0c-4f18-9f77-4e3f6a937cb1",
                    Name = "Technical Account",
                    NormalizedName = "TECHNICAL ACCOUNT"
                }, 
                new IdentityRole
                {
                    Id = "9b832f7a-47f0-42c4-b6c8-92b3c28b0d8d",
                    Name = "Regular User",
                    NormalizedName = "REGULAR USER"
                },
                new IdentityRole
                {
                    Id = "d4e61e14-8d1a-4a77-8c63-23e4d41b566a",
                    Name = "Viewer",
                    NormalizedName = "VIEWER"
                }
                );
        }
    }
}
