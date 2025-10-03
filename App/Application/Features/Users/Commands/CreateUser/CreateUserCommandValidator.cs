using Application.Interfaces;
using FluentValidation;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandValidator(IUserRepository userRepository)
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

        _userRepository = userRepository;
    }
}