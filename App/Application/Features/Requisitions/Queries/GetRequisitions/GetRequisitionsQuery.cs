using MediatR;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitions;
public record GetRequisitionsQuery() : IRequest<List<GetRequisitionDto>>;