using Domain.Common;

namespace Domain.Entities.Users;

public class RolePermission : BaseEntity<int>
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}