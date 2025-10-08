using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using MediatR;

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

        if (userToDelete == null)
            throw new NotFoundException(nameof(userToDelete), request.Id);

        await _userRepository.DeleteAsync(userToDelete);

        return Unit.Value;
    }
}