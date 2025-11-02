using Application.Features.Identity.Models;
using Domain.Entities.Users;

namespace Application.Interfaces.Identity;

public interface IPermissionService
{
    Task<IReadOnlyList<Permission>> GetAllPermissionsAsync();
    Task<IReadOnlyList<RolePermissionsDto>> GetAllRolesWithPermissionsAsync();
    Task<IReadOnlyList<Permission>> GetPermissionsByRoleNameAsync(string roleName);
    Task<IReadOnlyList<Permission>> GetPermissionsByUserIdAsync(string userId);
    Task AssignPermissionsToRoleAsync(string roleName, IEnumerable<string> permissionNames, string updatedBy);
    Task AssignPermissionsToUserAsync(string userId, IEnumerable<string> permissionNames, string updatedBy);
}

