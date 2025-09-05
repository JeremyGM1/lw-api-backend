using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity<string>
{
    public override required string Id { get; set; } = new Guid().ToString();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? IdentityUserId { get; private set; }

    public void LinkToIdentity(string identityUserId) => IdentityUserId = identityUserId;
}