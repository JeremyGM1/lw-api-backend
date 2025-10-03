using Application.Interfaces;
using FluentValidation;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(70).WithMessage("{PropertyName} must be fewer than 70 characters");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} must be fewer than 70 characters");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .EmailAddress().WithMessage("{PropertyName} must have a valid email address");

        RuleFor(p => p.Id)
            .MustAsync(UserExist).WithMessage("{PropertyName} does not exist.");

        _userRepository = userRepository;
    }

    private async Task<bool> UserExist(string id, CancellationToken token)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null;
    }
}