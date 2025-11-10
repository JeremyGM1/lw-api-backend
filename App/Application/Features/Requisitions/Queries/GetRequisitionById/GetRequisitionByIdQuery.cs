using MediatR;
using Application.Features.Requisitions.Dtos;
using Application.Common.Results;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string ReqCode) : IRequest<Result<GetRequisitionDto>>;