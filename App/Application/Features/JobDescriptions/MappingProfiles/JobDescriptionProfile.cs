using AutoMapper;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Domain.Entities.JobDescriptions;
using Application.Features.JobDescriptions.Dtos.Response;

namespace Application.Features.JobDescriptions.MappingProfiles;
public class JobDescriptionProfile : Profile
{
    public JobDescriptionProfile()
    {
        CreateMap<CreateJDCommand, JobDescription>();
        CreateMap<JobDescription, CreateJobDescriptionResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Requisition.Status))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.Requisition.LocationType));
        CreateMap <JobDescription, GetJobDescriptionResponseDto>()
            .ForMember(dest => dest.ReqCode, opt => opt.MapFrom(src => src.RequisitionCode))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Requisition.Status.Status))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.Requisition.LocationType.LocationType));
    }
}