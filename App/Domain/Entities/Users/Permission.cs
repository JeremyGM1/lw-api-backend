using Domain.Common;

namespace Domain.Entities.Users;

public class Permission : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}