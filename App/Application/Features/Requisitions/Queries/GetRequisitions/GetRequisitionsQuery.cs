using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos;

namespace Application.Features.Requisitions.Queries.GetRequisitions;
public record GetRequisitionsQuery() : IRequest<List<GetRequisitionDto>>;