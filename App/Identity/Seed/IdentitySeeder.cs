using Application.Interfaces.Features.Identity;
using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<AdminSettings> adminSettings,
            IPermissionService permissionService,
            IPermissionRepository permissionRepository)
        {
            foreach (var roleName in SystemRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                        Description = SystemRoles.RoleDescriptions[roleName]
                    };
                    await roleManager.CreateAsync(role);
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
                    await userManager.AddToRoleAsync(adminUser, SystemRoles.SuperAdministrator);
                else
                    throw new Exception($"Error creating admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            await PermissionSeeder.SeedAsync(adminSettings,permissionService, permissionRepository, roleManager, userManager);
        }
    }
}
