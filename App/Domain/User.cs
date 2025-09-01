namespace Domain;

public class User
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}
