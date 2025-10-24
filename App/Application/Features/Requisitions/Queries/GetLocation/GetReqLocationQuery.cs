using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetLocation;

public class GetReqLocationQuery : IRequest<List<GetReqLocationDto>>;