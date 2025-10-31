using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetLocationTypes;
public class GetReqLocationTypeQuery : IRequest<List<GetReqLocationTypeDto>>;