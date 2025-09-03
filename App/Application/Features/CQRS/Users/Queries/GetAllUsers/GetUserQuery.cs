using MediatR;

using Application.Features.CQRS.Dtos;

namespace Application.Features.CQRS.Users.Queries.GetAllUsers;

public record GetUserQuery : IRequest<List<UserDto>>;