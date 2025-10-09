using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AdminSettings> adminSettings)
        {
            foreach (var roleName in SystemRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = adminSettings.Value.Email;
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = adminSettings.Value.FirstName,
                    LastName = adminSettings.Value.LastName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminSettings.Value.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SystemRoles.SuperAdministrator);
                }
                else
                {
                    throw new Exception($"Error creating admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
