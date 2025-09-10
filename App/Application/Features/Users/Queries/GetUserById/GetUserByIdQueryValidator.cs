using FluentValidation;
using Persistence.Interfaces;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryValidator(IUserRepository userRepository)
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
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