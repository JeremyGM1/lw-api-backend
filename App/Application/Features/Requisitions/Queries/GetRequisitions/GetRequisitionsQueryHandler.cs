using Application.Features.Shared.Dtos.RequisitionDtos;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using LoggingService.Interfaces;

namespace Application.Features.Requisitions.Queries.GetRequisitions;
public class GetRequisitionsQueryHandler(IMapper mapper, IRequisitionRepository reqRepository, IAppLogger<GetRequisitionsQuery> logger) : IRequestHandler<GetRequisitionsQuery, List<GetRequisitionDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IAppLogger<GetRequisitionsQuery> _logger = logger;

    public async Task<List<GetRequisitionDto>> Handle(GetRequisitionsQuery request, CancellationToken cancellationToken)
    {
        var requisitions = await _reqRepository.GetAsync();

        var data = _mapper.Map<List<GetRequisitionDto>>(requisitions);

        _logger.LogInformation("Requisitions were retrieced successfully");

        return data;
    }
}