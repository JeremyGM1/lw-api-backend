using AutoMapper;

using Application.Features.CQRS.Dtos;
using Domain.AccessControl;

namespace Application.Features.CQRS.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<User, UserDto>();
    }
}