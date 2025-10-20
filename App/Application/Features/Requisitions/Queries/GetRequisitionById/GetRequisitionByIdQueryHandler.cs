using Application.Exceptions;
using Application.Features.Requisitions.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<GetRequisitionDto> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetRequisitionById(request.Id);

        if (requisition == null)
            throw new NotFoundException(nameof(requisition), request.Id);

        return _mapper.Map<GetRequisitionDto>(requisition);
    }
}