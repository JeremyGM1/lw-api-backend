using MediatR;
using Application.Exceptions;
using Domain.Entities;
using Persistence.Interfaces;
using Application.Features.Users.Commands.DeleteUser;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {

        var validator = new DeleteUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid User", validationResult);

        var userToDelete = await _userRepository.GetByIdAsync(request.Id);

        await _userRepository.DeleteAsync(userToDelete);

        return Unit.Value;
    }
}