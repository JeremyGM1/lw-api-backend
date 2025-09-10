using FluentValidation;
using Persistence.Interfaces;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{PropertyName} can not be empty")
            .NotNull()
            .MustAsync(UserExist).WithMessage("{PropertyName} does not exist.");

        _userRepository = userRepository;
    }

    private async Task<bool> UserExist(string id, CancellationToken token)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null;
    }
}