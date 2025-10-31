using Domain.Entities.Users;

namespace Application.Interfaces;
public interface IPermissionRepository : IGenericRepository<Permission, Guid>
{
    Task<IReadOnlyList<Permission>> GetPermissionsByRoleNameAsync(string roleName);
    Task<IReadOnlyList<Permission>> GetPermissionsByUserIdAsync(string userId);
    Task AssignPermissionsToRoleAsync(string roleId, IEnumerable<Guid> permissionIds, string updatedBy);
    Task AssignPermissionsToUserAsync(string userId, IEnumerable<Guid> permissionIds, string updatedBy);
}
