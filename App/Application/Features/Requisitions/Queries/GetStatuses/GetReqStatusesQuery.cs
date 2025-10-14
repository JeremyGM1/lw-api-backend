using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetStatuses;
public record GetReqStatusesQuery : IRequest<List<GetReqStatusesDto>>;