using Application.Features.Requisitions.Dtos.Lookup;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetInterval;

public class GetReqIntervalQuery : IRequest<List<GetReqIntervalDto>>;