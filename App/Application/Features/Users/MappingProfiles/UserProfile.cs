using AutoMapper;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.UpdateUser;
using Domain.Entities.Users;
using Application.Features.Users.Dtos;

namespace Application.Features.Users.MappingProfiles;

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