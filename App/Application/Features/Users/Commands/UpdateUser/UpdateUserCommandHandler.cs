using AutoMapper;
using MediatR;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Users;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id!);

        var validator = new UpdateUserCommandValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid operation", validationResult);

        var userToUpdate = _mapper.Map<User>(request);

        userToUpdate.CreatedAt = user!.CreatedAt;
        userToUpdate.CreatedBy = user!.CreatedBy;
        userToUpdate.UpdatedAt = DateTime.Now;
        userToUpdate.UpdatedBy = "Admin";

        await _userRepository.UpdateAsync(userToUpdate);

        return Unit.Value;
    }
}