using Domain.AccessControl;

namespace Domain.AccessControl;
public class UserRole : BaseEntity
{
    public required string UserId { get; set; }
    public required string RoleId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}