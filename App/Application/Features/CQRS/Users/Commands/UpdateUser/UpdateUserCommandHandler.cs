using AutoMapper;
using MediatR;

using Application.Exceptions;
using Application.Interfaces.Persistence;
using Domain.AccessControl;

namespace Application.Features.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        //Convert to domain entity object
        var userToCreate = _mapper.Map<User>(request);

        // Validate incoming data
        if (userToCreate == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        //Add to database
        await _userRepository.CreateAsync(userToCreate);

        //Return record id
        return Unit.Value;
    }
}
