using MediatR;

using Application.Exceptions;
using Application.Interfaces.Persistence;
using Domain.AccessControl;

namespace Application.Features.CQRS.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //Convert to domain entity object
        var userToDelete = await _userRepository.GetByIdAsync(request.Id);

        // Validate that record exist
        if (userToDelete == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        //Remove from database
        await _userRepository.DeleteAsync(userToDelete);

        //Return record id
        return Unit.Value;
    }
}