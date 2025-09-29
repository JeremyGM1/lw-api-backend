using MediatR;
using Application.Features.Shared.Dtos.UserDtos;

namespace Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<UserDto>;