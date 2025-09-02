using AutoMapper;
using MediatR;

using Application.Exceptions;
using Application.Interfaces.Persistence;
using Domain.AccessControl;

namespace Application.Features.CQRS.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<CreateUserCommand, string>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new CreateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid User", validationResult);

        //Convert to domain entity object
        var userToCreate = _mapper.Map<User>(request);

        //Update database
        await _userRepository.CreateAsync(userToCreate);

        //Return record id
        return userToCreate.Id;
    }
}
