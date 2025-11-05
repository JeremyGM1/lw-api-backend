using Application.Exceptions;
using Application.Features.Identity.Models.RoleService;
using Application.Interfaces.Features.Identity;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new NotFoundException($"The role '{roleName}' was not found.", roleName);

            return new RoleDto(role.Name!, role.Description);
        }

        public async Task<bool> UpdateRoleDescriptionAsync(string roleName, string description)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new NotFoundException($"The role '{roleName}' was not found.", roleName);

            role.Description = description;
            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded;
        }
    }
}
