namespace Domain.AccessControl;

public class Permission : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}