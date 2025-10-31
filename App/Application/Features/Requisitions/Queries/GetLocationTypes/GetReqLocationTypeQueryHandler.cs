using Application.Features.Requisitions.Dtos.Lookup;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.Features.Requisitions.Lookup;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetLocationTypes;
public class GetReqLocationTypeQueryHandler(IMapper mapper, IReqLocationTypeRepository reqLocationTypeRepository) : IRequestHandler<GetReqLocationTypeQuery, List<GetReqLocationTypeDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqLocationTypeRepository _reqLocationTypeRepository = reqLocationTypeRepository;
    public async Task<List<GetReqLocationTypeDto>> Handle(GetReqLocationTypeQuery request, CancellationToken cancellationToken)
    {
        var locations = await _reqLocationTypeRepository.GetLocationTypes();

        var data = _mapper.Map<List<GetReqLocationTypeDto>>(locations);

        return data;
    }
}