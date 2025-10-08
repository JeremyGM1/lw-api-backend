using System.Runtime.CompilerServices;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using Application.Interfaces;
using Application.Interfaces.Lookup;
using AutoMapper;
using MediatR;

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