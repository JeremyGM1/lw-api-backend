using Application.Common.Results;
using Application.Features.Requisitions.Dtos.Response;
using Application.Interfaces.Features.Requisitions;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetRequisitionsDropdown;
public class GetRequisitionsDropdownQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionsDropdownQuery, Result<List<GetRequisitionsDropdownResponseDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    public async Task<Result<List<GetRequisitionsDropdownResponseDto>>> Handle(GetRequisitionsDropdownQuery request, CancellationToken cancellationToken)
    {
        var reqs = await _reqRepository.GetRequisitions();

        var result = _mapper.Map<List<GetRequisitionsDropdownResponseDto>>(reqs);

        return Result<List<GetRequisitionsDropdownResponseDto>>.Success(result);
    }
}