using Application.Interfaces.Features.Identity;
using Identity.Configurations;
using Identity.Models;
using Identity.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedIdentityAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var adminSettings = scope.ServiceProvider.GetRequiredService<IOptions<AdminSettings>>();
            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
            var permissionRepository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

            await IdentitySeeder.SeedAsync(userManager, roleManager, adminSettings, permissionService, permissionRepository);
        }
    }
}
