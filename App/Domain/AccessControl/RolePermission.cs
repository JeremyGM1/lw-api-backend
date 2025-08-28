namespace Domain.AccessControl;

public class RolePermission : BaseEntity
{
    public required string RoleId { get; set; }
    public required string PermissionId { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}


