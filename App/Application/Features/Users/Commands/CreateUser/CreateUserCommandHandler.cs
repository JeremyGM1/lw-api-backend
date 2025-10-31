using Application.Exceptions;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Users;
using AutoMapper;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository, ICommonHelpers commonHelpers) : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid User", validationResult);

        var userToCreate = _mapper.Map<User>(request);

        userToCreate.Id = Guid.NewGuid().ToString();
        userToCreate.CreatedAt = DateTime.UtcNow.Date;
        userToCreate.CreatedBy = "Admin";

        await _userRepository.CreateAsync(userToCreate);

        return Unit.Value;
    }
}