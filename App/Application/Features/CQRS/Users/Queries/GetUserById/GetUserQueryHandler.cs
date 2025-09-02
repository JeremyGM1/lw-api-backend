using AutoMapper;
using MediatR;

using Application.Features.CQRS.Dtos;
using Application.Interfaces.Persistence;

namespace Application.Features.CQRS.Users.Queries.GetUserById;

public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // Query the db
        var user = await _userRepository.GetByIdAsync(request.Id);

        // Convert data objects to DTO objects
        var data = _mapper.Map<UserDto>(user);

        // return list of DTO object
        return data;
    }
}