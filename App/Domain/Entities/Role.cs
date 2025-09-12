using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required ICollection<UserRole> UserRoles { get; set; } = [];
}