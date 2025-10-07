using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string Id) : IRequest<GetRequisitionDto>;