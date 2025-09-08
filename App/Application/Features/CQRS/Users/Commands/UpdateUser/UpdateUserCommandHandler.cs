using AutoMapper;
using MediatR;

using Application.Exceptions;
using Application.Interfaces.Logging;
using Domain.AccessControl;
using Persistence.Interfaces;

namespace Application.Features.CQRS.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<UpdateUserCommandHandler> logger)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userToUpdate = await _userRepository.GetByIdAsync(request.Id);

        var validator = new UpdateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        try
        {
            _mapper.Map(request, userToUpdate);

            await _userRepository.UpdateAsync(userToUpdate);

        }catch(BadRequestException ex)
        {
            _logger.LogWarning("Validation errors in update request for {0} - {1}. Error Message: {2}", nameof(User), request.Id, ex.Message);
        }
        return Unit.Value;
    }
}
