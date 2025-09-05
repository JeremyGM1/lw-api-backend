using FluentValidation;
using Persistence.Interfaces;

namespace Application.Features.CQRS.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private IUserRepository userRepository;

    public UpdateUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull();

        this._userRepository = userRepository;
    }
}
