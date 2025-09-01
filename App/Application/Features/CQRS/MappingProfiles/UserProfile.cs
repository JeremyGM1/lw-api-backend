using Application.Features.CQRS.Dtos;
using AutoMapper;
using Domain;

namespace Application.Features.CQRS.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
    }
}