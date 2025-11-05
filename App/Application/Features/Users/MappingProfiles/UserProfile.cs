using AutoMapper;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Dtos;
using Domain.Entities.Users;

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