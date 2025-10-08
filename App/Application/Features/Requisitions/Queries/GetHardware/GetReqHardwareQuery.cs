using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetHardware;

public class GetReqHardwareQuery : IRequest<List<GetReqHardwareDto>>;