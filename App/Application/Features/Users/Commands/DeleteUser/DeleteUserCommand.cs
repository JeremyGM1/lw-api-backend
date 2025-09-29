using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public required string Id { get; set; }
}