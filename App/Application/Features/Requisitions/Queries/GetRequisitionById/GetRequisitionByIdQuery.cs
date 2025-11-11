using MediatR;
using Application.Common.Results;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public record GetRequisitionByIdQuery(string ReqCode) : IRequest<Result<GetRequisitionDto>>;