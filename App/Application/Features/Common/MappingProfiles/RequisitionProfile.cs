using AutoMapper;
using Application.Features.Common.Dtos.RequisitionDtos;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Domain.Entities.Requisitions;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;

namespace Application.Features.Common.MappingProfiles;

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
    }
}