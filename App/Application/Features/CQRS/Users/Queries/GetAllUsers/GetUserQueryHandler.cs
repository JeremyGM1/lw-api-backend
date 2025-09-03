using AutoMapper;
using MediatR;

using Application.Features.CQRS.Dtos;
using Application.Interfaces.Persistence;
using Application.Interfaces.Logging;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers;

public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository, IAppLogger<GetUserQueryHandler> logger) : IRequestHandler<GetUserQuery, List<UserDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppLogger<GetUserQueryHandler> _logger = logger;

    public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // TODO Query the db
        var user = await _userRepository.GetAsync();

        // TODO Convert data objects to DTO objects
        var data = _mapper.Map<List<UserDto>>(user);

        // TODO return list of DTO object
        _logger.LogInformation("Leave types were retrieved successfully");
        return data;
    }
}