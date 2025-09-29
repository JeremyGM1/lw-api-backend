using Domain.Common;

namespace Domain.Entities.Users;

public class Role : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required ICollection<UserRole> UserRoles { get; set; } = [];
}