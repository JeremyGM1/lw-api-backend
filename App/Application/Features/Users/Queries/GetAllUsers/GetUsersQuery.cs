using MediatR;
using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetUsersQuery : IRequest<List<UserDto>>;