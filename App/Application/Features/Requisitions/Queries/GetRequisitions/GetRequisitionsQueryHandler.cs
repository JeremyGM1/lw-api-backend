using AutoMapper;
using MediatR;
using Application.Interfaces;
using LoggingService.Interfaces;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitions;

public class GetRequisitionsQueryHandler(IMapper mapper, IRequisitionRepository reqRepository, IAppLogger<GetRequisitionsQuery> logger) : IRequestHandler<GetRequisitionsQuery, List<GetRequisitionDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IAppLogger<GetRequisitionsQuery> _logger = logger;

    public async Task<List<GetRequisitionDto>> Handle(GetRequisitionsQuery request, CancellationToken cancellationToken)
    {
        var requisitions = await _reqRepository.GetRequisitions();

        var data = _mapper.Map<List<GetRequisitionDto>>(requisitions);

        _logger.LogInformation("Requisitions were retrieced successfully");

        return data;
    }
}