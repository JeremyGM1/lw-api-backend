using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetWorkTypes;

public class GetReqWorkTypeQuery : IRequest<List<GetReqWorkTypeDto>>;