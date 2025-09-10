using MediatR;
using Application.Features.Shared.Dtos.UserDtos;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetUserQuery : IRequest<List<UserDto>>;