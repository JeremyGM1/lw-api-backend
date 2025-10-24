using AutoMapper;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.Dtos.Lookup;
using Application.Features.Requisitions.Dtos;
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
            .ForMember(dest => dest.HardwareName, opt => opt.MapFrom(src => src.Hardware.Hardware));
        CreateMap<CreateRequisitionCommand, Requisition>();
        CreateMap<UpdateRequisitionCommand, Requisition>();
        CreateMap<UpdateRequisitionDto, UpdateRequisitionCommand>();
        CreateMap<DeleteRequisitionCommand, Requisition>();

        CreateMap<RequisitionStatus, GetReqStatusesDto>();
        CreateMap<RequisitionWorkType, GetReqWorkTypeDto>();
        CreateMap<RequisitionHardware, GetReqHardwareDto>();

        CreateMap<RequisitionLocation, GetReqLocationDto>();
        CreateMap<RequisitionCurrency, GetReqCurrencyDto>();
        CreateMap<RequisitionInterval, GetReqIntervalDto>();
    }
}