namespace Domain;
public class UserRole
{
    public required string UserId { get; set; }
    public required string RoleId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}