using AutoMapper;
using MediatR;
using Application.Exceptions;
using Application.Features.Shared.Dtos.UserDtos;
using Persistence.Interfaces;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetUserByIdQueryValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException($"Invalid operation {validationResult}", validationResult);

        var user = await _userRepository.GetByIdAsync(request.Id);

        return _mapper.Map<UserDto>(user);
    }
}