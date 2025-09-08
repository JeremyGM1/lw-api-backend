using MediatR;

namespace Application.Features.CQRS.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<string>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}