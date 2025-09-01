using Application.Features.CQRS.Users.Dtos;
using MediatR;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers;

public record GetUserQuery : IRequest<List<UserDto>>;