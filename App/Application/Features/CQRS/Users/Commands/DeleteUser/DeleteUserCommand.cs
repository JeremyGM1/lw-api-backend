using MediatR;

namespace Application.Features.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public required string Id { get; set; }
}