using MediatR;
using Application.Features.Users.Dtos;

namespace Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<UserDto>;