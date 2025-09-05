using AutoMapper;
using MediatR;

using Application.Exceptions;
using Application.Interfaces.Logging;
using Domain.AccessControl;
using Persistence.Interfaces;

namespace Application.Features.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<CreateUserCommandHandler> logger)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);      

        if (validationResult.Errors.Count != 0)
        {
            _logger.LogWarning("Validator error in create request for {0} - {1}", nameof(User), request.Id);
            throw new BadRequestException($"Invalid User {validationResult}", validationResult);
        }

        var userToCreate = _mapper.Map<User>(request);

        userToCreate.CreatedAt = new DateTime();
        userToCreate.CreatedBy = "Admin";

        await _userRepository.CreateAsync(userToCreate);

        return userToCreate.Id;
    }
}