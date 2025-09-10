using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Unit>
{
    [JsonIgnore]
    public string? Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}