using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetWorkTypes;
public class GetReqWorkTypeQuery : IRequest<List<GetReqWorkTypeDto>>;