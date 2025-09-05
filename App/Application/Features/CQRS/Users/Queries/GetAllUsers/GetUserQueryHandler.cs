using AutoMapper;
using MediatR;

using Application.Features.CQRS.Dtos;
using Application.Interfaces.Logging;
using Persistence.Interfaces;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers;

public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<GetUserQueryHandler> logger) : IRequestHandler<GetUserQuery, List<UserDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppLogger<GetUserQueryHandler> _logger = logger;

    public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync();

        var data = _mapper.Map<List<UserDto>>(user);

        _logger.LogInformation("Leave types were retrieved successfully");
        return data;
    }
}