using MediatR;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string Id) : IRequest<GetRequisitionDto>;