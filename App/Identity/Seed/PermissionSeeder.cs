using Application.Interfaces;
using Application.Interfaces.Identity;
using Domain.Entities.Users;
using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Seed
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(
            IOptions<AdminSettings> adminSettings,
            IPermissionService permissionService,
            IPermissionRepository permissionRepository,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // Create base permissions only if the database is empty
            var existingPermissions = await permissionRepository.GetAsync();

            if (!existingPermissions.Any())
            {
                var allPermissions = SystemPermissions.AllPermissions.Select(p => new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = p.Name,
                    Description = p.Description,
                    CreatedBy = "system",
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                foreach (var permission in allPermissions)
                    await permissionRepository.CreateAsync(permission);

                existingPermissions = allPermissions;
            }

            // Assign default permissions to each predefined role
            foreach (var (roleName, permissionNames) in SystemPermissions.RolePermissions)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
                await permissionService.AssignPermissionsToRoleAsync(roleName, permissionNames, "system");
            }

            // Assign all permissions to the Super Administrator account
            var superAdminEmail = adminSettings.Value.Email;
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin != null)
            {
                var superAdminRole = await roleManager.FindByNameAsync(SystemRoles.SuperAdministrator);
                if (superAdminRole == null)
                {
                    superAdminRole = new IdentityRole(SystemRoles.SuperAdministrator);
                    await roleManager.CreateAsync(superAdminRole);
                }
                // Ensure the Super Admin user has the Super Administrator role
                if (!await userManager.IsInRoleAsync(superAdmin, SystemRoles.SuperAdministrator))
                    await userManager.AddToRoleAsync(superAdmin, SystemRoles.SuperAdministrator);

                // Assign every permission available to the Super Admin by name
                var allPermissionNames = SystemPermissions.AllPermissions.Select(p => p.Name).ToList();
                await permissionService.AssignPermissionsToUserAsync(superAdmin.Id, allPermissionNames, "system");
            }
        }
    }
} 


