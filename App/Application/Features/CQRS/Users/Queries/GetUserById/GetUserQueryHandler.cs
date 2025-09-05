using AutoMapper;
using MediatR;

using Application.Features.CQRS.Dtos;
using Persistence.Interfaces;

namespace Application.Features.CQRS.Users.Queries.GetUserById;

public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        var data = _mapper.Map<UserDto>(user);

        return data;
    }
}