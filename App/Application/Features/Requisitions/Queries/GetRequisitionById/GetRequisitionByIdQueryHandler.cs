using AutoMapper;
using MediatR;
using Application.Interfaces;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<GetRequisitionDto> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetRequisitionById(request.Id);

        return _mapper.Map<GetRequisitionDto>(requisition);
    }
}