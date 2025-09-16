using AutoMapper;
using MediatR;
using Application.Exceptions;
using LoggingService.Interfaces;
using Persistence.Interfaces;
using Domain.Entities.Users;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppLogger<CreateUserCommandHandler> _logger = logger;

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