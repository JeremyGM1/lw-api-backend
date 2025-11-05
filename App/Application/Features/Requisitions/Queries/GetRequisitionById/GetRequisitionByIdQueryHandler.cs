using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Features.Requisitions.Dtos;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.Features.Common;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<GetRequisitionDto> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetRequisitionById(request.ReqCode);

        if (requisition == null)
            throw new NotFoundException(nameof(requisition), request.ReqCode);

        return _mapper.Map<GetRequisitionDto>(requisition);
    }
}