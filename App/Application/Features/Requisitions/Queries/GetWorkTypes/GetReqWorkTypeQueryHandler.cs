using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using Application.Interfaces.Lookup;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetWorkTypes;
public class GetReqWorkTypeQueryHandler(IMapper mapper, IReqWorkTypeRepository reqWorkTypeRepository) : IRequestHandler<GetReqWorkTypeQuery, List<GetReqWorkTypeDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqWorkTypeRepository _reqWorkTypeRepository = reqWorkTypeRepository;
    public async Task<List<GetReqWorkTypeDto>> Handle(GetReqWorkTypeQuery request, CancellationToken cancellationToken)
    {
        var workTypes = await _reqWorkTypeRepository.GetWorkTypes();

        var data = _mapper.Map<List<GetReqWorkTypeDto>>(workTypes);

        return data;
    }
}