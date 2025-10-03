using AutoMapper;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Shared.Dtos.RequisitionDtos;
using Domain.Entities.Requisitions;

namespace Application.Features.Shared.MappingProfiles;
public class RequisitionProfile : Profile
{
    public RequisitionProfile()
    {
        CreateMap<Requisition, CreateRequisitionDto>();
        CreateMap<Requisition, GetRequisitionDto>();
        CreateMap<CreateRequisitionCommand, Requisition>();
        //CreateMap<UpdateRequisitionCommand, Requisition>();
    }
}