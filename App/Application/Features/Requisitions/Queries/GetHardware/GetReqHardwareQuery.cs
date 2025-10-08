using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetHardware;
public class GetReqHardwareQuery : IRequest<List<GetReqHardwareDto>>;