using Application.Interfaces.Features.Identity;
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
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
          
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

           
            foreach (var (roleName, permissionNames) in SystemPermissions.RolePermissions)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    var newRole = new ApplicationRole
                    {
                        Name = roleName,
                        Description = SystemRoles.RoleDescriptions.TryGetValue(roleName, out var desc)
                            ? desc
                            : "Default system role"
                    };

                    await roleManager.CreateAsync(newRole);
                }
                await permissionService.AssignPermissionsToRoleAsync(roleName, permissionNames, "system");
            }

           
            var superAdminEmail = adminSettings.Value.Email;
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin != null)
            {
                var superAdminRole = await roleManager.FindByNameAsync(SystemRoles.SuperAdministrator);
                if (superAdminRole == null)
                {
                    superAdminRole = new ApplicationRole
                    {
                        Name = SystemRoles.SuperAdministrator,
                        Description = SystemRoles.RoleDescriptions[SystemRoles.SuperAdministrator]
                    };
                    await roleManager.CreateAsync(superAdminRole);
                }
             
                if (!await userManager.IsInRoleAsync(superAdmin, SystemRoles.SuperAdministrator))
                    await userManager.AddToRoleAsync(superAdmin, SystemRoles.SuperAdministrator);

           
                var allPermissionNames = SystemPermissions.AllPermissions.Select(p => p.Name).ToList();
                await permissionService.AssignPermissionsToUserAsync(superAdmin.Id, allPermissionNames, "system");
            }
        }
    }
} 


