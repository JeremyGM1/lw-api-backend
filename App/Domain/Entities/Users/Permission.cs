using Domain.Common;

namespace Domain.Entities.Users;
public class Permission : BaseEntity<int>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
