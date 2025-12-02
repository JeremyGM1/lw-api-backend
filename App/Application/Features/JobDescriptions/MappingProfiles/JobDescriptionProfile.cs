using AutoMapper;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Domain.Entities.JobDescriptions;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Features.JobDescriptions.Commands.UpdateJDCommand;

namespace Application.Features.JobDescriptions.MappingProfiles;
public class JobDescriptionProfile : Profile
{
    public JobDescriptionProfile()
    {
        CreateMap<CreateJDCommand, JobDescription>();
        CreateMap<JobDescription, CreateJobDescriptionResponseDto>()
            .ForMember(dest => dest.ReqCode, opt => opt.MapFrom(src => src.ReqCode))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Requisition.Status.Status))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.Requisition.LocationType.LocationType));
        CreateMap<JobDescription, GetJobDescriptionResponseDto>()
            .ForMember(dest => dest.ReqCode, opt => opt.MapFrom(src => src.ReqCode))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Requisition.Status.Status))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.Requisition.LocationType.LocationType));
        CreateMap<UpdateJDCommand, JobDescription>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<JobDescription, UpdateJobDescriptionResponseDto>();
        CreateMap<JobDescriptionLog, GetJobDescriptionLogsResponseDto>();
    }
}