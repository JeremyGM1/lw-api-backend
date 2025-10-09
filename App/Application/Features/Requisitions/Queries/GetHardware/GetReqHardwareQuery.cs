using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetHardware;

public class GetReqHardwareQuery : IRequest<List<GetReqHardwareDto>>;