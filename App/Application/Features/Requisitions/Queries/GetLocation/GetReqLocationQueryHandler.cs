using AutoMapper;
using MediatR;
using Application.Interfaces.Lookup.Requisition;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetLocation;
public class GetReqLocationQueryHandler(IMapper mapper, IReqLocationRepository reqLocationRepository) : IRequestHandler<GetReqLocationQuery, List<GetReqLocationDto>>
{
    private readonly IMapper _mappper = mapper;
    private readonly IReqLocationRepository _reqLocationRepository = reqLocationRepository;

    public async Task<List<GetReqLocationDto>> Handle(GetReqLocationQuery request, CancellationToken cancellationToken)
    {
        var locations = await _reqLocationRepository.GetLocations();

        var data = _mappper.Map<List<GetReqLocationDto>>(locations);

        return data;
    }
}