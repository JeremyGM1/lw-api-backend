using Application.Features.CQRS.Dtos;
using MediatR;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers;

public record GetUserQuery : IRequest<List<UserDto>>;