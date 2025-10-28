using AutoMapper;
using MediatR;
using Application.Interfaces.Lookup.Requisition;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetInterval;
public class GetReqIntervalQueryHandler(IMapper mapper, IReqIntervalRepository reqIntervalRepository) : IRequestHandler<GetReqIntervalQuery, List<GetReqIntervalDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqIntervalRepository _reqIntervalRepository = reqIntervalRepository;

    public async Task<List<GetReqIntervalDto>> Handle(GetReqIntervalQuery request, CancellationToken cancellationToken)
    {
        var intervals = await _reqIntervalRepository.GetInterval();

        var data = _mapper.Map<List<GetReqIntervalDto>>(intervals);

        return data;
    }
}