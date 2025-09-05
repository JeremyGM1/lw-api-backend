using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity<string>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}