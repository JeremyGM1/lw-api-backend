using MediatR;

using Application.Features.CQRS.Dtos;

namespace Application.Features.CQRS.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<UserDto>;