using MediatR;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string ReqCode) : IRequest<GetRequisitionDto>;