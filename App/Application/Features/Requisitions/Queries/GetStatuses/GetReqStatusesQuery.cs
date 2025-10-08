using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetStatuses;
public record GetReqStatusesQuery : IRequest<List<GetReqStatusesDto>>;