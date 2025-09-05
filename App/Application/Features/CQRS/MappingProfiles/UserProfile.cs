using AutoMapper;

using Application.Features.CQRS.Dtos;
using Application.Features.CQRS.Users.Commands.CreateUser;
using Application.Features.CQRS.Users.Commands.UpdateUser;
using Domain.AccessControl;

namespace Application.Features.CQRS.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<User, UserDto>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
    }
}