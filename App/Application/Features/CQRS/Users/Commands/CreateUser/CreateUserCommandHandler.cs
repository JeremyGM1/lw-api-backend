using AutoMapper;
using MediatR;

using Application.Exceptions;
using Application.Interfaces.Persistence;
using Domain.AccessControl;
using Application.Interfaces.Logging;

namespace Application.Features.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, string>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppLogger<CreateUserCommandHandler> _logger = logger;

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new CreateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            _logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(User), request.Id);
            throw new BadRequestException("Invalid User", validationResult);
        }

        //Convert to domain entity object
        var userToCreate = _mapper.Map<User>(request);

        //Update database
        await _userRepository.CreateAsync(userToCreate);

        //Return record id
        return userToCreate.Id;
    }
}
