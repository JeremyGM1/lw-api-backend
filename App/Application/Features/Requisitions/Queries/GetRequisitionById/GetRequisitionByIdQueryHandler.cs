using AutoMapper;
using MediatR;
using Application.Exceptions;
using Application.Features.Requisitions.Dtos;
using Application.Interfaces.Features.Requisitions;
using Application.Common.Results;
using Application.Common.Results.RequisitionErrors;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionByIdQuery, Result<GetRequisitionDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<Result<GetRequisitionDto>> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetRequisitionById(request.ReqCode);

        if (requisition == null)
            return Result<GetRequisitionDto>.Failure(RequisitionErrors.NotExistingRequisition(request.ReqCode));

        var result = _mapper.Map<GetRequisitionDto>(requisition);
        return Result<GetRequisitionDto>.Success(result);
    }
}