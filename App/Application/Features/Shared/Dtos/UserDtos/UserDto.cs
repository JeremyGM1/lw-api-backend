namespace Application.Features.Shared.Dtos.UserDtos;

public class UserDto
{
    public virtual string Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}