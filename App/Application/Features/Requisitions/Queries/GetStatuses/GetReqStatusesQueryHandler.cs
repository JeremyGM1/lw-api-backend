using AutoMapper;
using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;
using Application.Interfaces.Features.Lookup.Requisition;

namespace Application.Features.Requisitions.Queries.GetStatuses;

public class GetReqStatusesQueryHandler(IMapper mapper, IReqStatusRepository reqStatusRepository) : IRequestHandler<GetReqStatusesQuery, List<GetReqStatusesDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqStatusRepository _reqStatusRepository = reqStatusRepository;

    public async Task<List<GetReqStatusesDto>> Handle(GetReqStatusesQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _reqStatusRepository.GetStatuses();

        var data = _mapper.Map<List<GetReqStatusesDto>>(statuses);

        return data;
    }
}