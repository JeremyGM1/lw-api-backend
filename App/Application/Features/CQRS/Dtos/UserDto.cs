namespace Application.Features.CQRS.Dtos;

public class UserDto
{
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}