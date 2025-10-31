using Domain.Common;

namespace Domain.Entities.Users;

public class RolePermission : BaseEntity<Guid>
{
    public string RoleId { get; set; } = default!;
    public Guid PermissionId { get; set; } = default!;

    public Permission Permission { get; set; } = default!;
}
