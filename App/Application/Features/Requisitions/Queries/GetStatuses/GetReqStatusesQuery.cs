using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetStatuses;
public record GetReqStatusesQuery : IRequest<List<GetReqStatusesDto>>;