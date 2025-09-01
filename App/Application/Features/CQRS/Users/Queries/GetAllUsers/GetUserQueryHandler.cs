using Application.Features.CQRS.Users.Dtos;
using Application.Interfaces.Persistence;
using AutoMapper;
using MediatR;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers
{
    public class GetUserQueryHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<GetUserQuery, List<UserDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // TODO Query the db
            var user = await _userRepository.GetAsync();

            // TODO Convert data objects to DTO objects
            var data = _mapper.Map<List<UserDto>>(user);

            // TODO return list of DTO object
            return data;
        }
    }
}