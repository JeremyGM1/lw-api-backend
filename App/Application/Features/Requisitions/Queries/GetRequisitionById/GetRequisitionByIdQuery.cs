using MediatR;
using Application.Features.Shared.Dtos.RequisitionDtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string Id) : IRequest<GetRequisitionDto>;