using AutoMapper;
using MediatR;
using Application.Features.Users.Dtos;
using Application.Interfaces.LoggingService;
using Application.Interfaces.Features.Users;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetUsersQueryHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<GetUsersQueryHandler> logger) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppLogger<GetUsersQueryHandler> _logger = logger;

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync();

        var data = _mapper.Map<List<UserDto>>(user);

        _logger.LogInformation("Leave types were retrieved successfully");
        return data;
    }
}