using Application.Features.JobDescriptions.Commands.UpdateJDCommand;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.Dtos;
using Application.Features.Requisitions.Dtos.Lookup;
using Application.Features.Requisitions.Dtos.Response;
using AutoMapper;
using Domain.Entities.JobDescriptions;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.MappingProfiles;

public class RequisitionProfile : Profile
{
    public RequisitionProfile()
    {
        CreateMap<Requisition, CreateRequisitionDto>();
        CreateMap<Requisition, GetRequisitionDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Status))
            .ForMember(dest => dest.WorkTypeName, opt => opt.MapFrom(src => src.WorkType.WorkType))
            .ForMember(dest => dest.HardwareName, opt => opt.MapFrom(src => src.Hardware.Hardware))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Location))
            .ForMember(dest => dest.LocationType, opt => opt.MapFrom(src => src.LocationType.LocationType))
            .ForMember(dest => dest.CompensationBandCurrency, opt => opt.MapFrom(src => src.CompensationBandCurrency.Currency))
            .ForMember(dest => dest.CompensationBandInterval, opt => opt.MapFrom(src => src.CompensationBandInterval.Interval));
        CreateMap<CreateRequisitionCommand, Requisition>();
        CreateMap<UpdateRequisitionCommand, Requisition>()
            .ForAllMembers(opt => opt.Condition(srcMember => srcMember != null));
        CreateMap<UpdateRequisitionDto, UpdateRequisitionCommand>();
        CreateMap<DeleteRequisitionCommand, Requisition>();

        CreateMap<RequisitionStatus, GetReqStatusesDto>();
        CreateMap<RequisitionWorkType, GetReqWorkTypeDto>();
        CreateMap<RequisitionHardware, GetReqHardwareDto>();

        CreateMap<RequisitionLocationType, GetReqLocationTypeDto>();
        CreateMap<RequisitionLocation, GetReqLocationDto>();
        CreateMap<RequisitionCurrency, GetReqCurrencyDto>();
        CreateMap<RequisitionInterval, GetReqIntervalDto>();

        CreateMap<RequisitionLog, GetRequisitionLogsDto>()
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")));

        CreateMap<Requisition, GetRequisitionsDropdownResponseDto>();
    }
}