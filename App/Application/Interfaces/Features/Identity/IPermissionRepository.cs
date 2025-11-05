using Application.Interfaces.Features;
using Domain.Entities.Users;

namespace Application.Interfaces.Features.Identity;
public interface IPermissionRepository : IGenericRepository<Permission, Guid>
{
    Task<IReadOnlyList<Permission>> GetPermissionsByRoleNameAsync(string roleName);
    Task<IReadOnlyList<Permission>> GetPermissionsByUserIdAsync(string userId);
    Task AssignPermissionsToRoleAsync(string roleId, IEnumerable<Guid> permissionIds, string updatedBy);
    Task AssignPermissionsToUserAsync(string userId, IEnumerable<Guid> permissionIds, string updatedBy);
    Task<IReadOnlyList<(string RoleName, string? RoleDescription, List<Permission> Permissions)>> GetAllRolesWithPermissionsAsync();
}
