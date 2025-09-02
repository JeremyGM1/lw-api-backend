using MediatR;

namespace Application.Features.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Unit>
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}