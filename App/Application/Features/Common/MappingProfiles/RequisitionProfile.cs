using Application.Features.Common.Dtos.RequisitionDtos;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Shared.Dtos.RequisitionDtos;
using Application.Features.Shared.Dtos.UserDtos;
using AutoMapper;
using Domain.Entities.Requisitions;
using Domain.Entities.Users;

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