using AutoMapper;
using Application.Features.Common.Dtos.RequisitionDtos;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Domain.Entities.Requisitions;

namespace Application.Features.Common.MappingProfiles;

public class RequisitionProfile : Profile
{
    public RequisitionProfile()
    {
        CreateMap<Requisition, CreateRequisitionDto>();
        CreateMap<Requisition, GetRequisitionDto>();
        CreateMap<CreateRequisitionCommand, Requisition>();
        CreateMap<UpdateRequisitionCommand, Requisition>();
        //CreateMap<UpdateRequisitionCommand, Requisition>();
    }
}