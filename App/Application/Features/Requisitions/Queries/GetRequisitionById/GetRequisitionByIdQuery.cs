using MediatR;
using Application.Features.Shared.Dtos.RequisitionDtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public class GetRequisitionByIdQuery(string Id) : IRequest<GetRequisitionDto>;