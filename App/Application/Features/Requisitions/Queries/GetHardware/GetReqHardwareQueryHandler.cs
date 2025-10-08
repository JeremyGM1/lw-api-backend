using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using Application.Interfaces.Lookup;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetHardware;
public class GetReqHardwareQueryHandler(IMapper mapper, IReqHardwareRepository reqHardwareRepository) : IRequestHandler<GetReqHardwareQuery, List<GetReqHardwareDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqHardwareRepository _reqHardwareRepository = reqHardwareRepository;
    public async Task<List<GetReqHardwareDto>> Handle(GetReqHardwareQuery request, CancellationToken cancellationToken)
    {
        var hardware = await _reqHardwareRepository.GetHardware();

        var data = _mapper.Map<List<GetReqHardwareDto>>(hardware);

        return data;
    }
}