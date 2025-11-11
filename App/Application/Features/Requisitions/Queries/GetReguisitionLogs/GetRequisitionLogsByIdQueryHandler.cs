using AutoMapper;
using MediatR;
using Application.Common.Results.RequisitionErrors;
using Application.Common.Results;
using Application.Features.Requisitions.Dtos;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Queries.GetReguisitionLogs;

public class GetRequisitionLogsByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionLogsByIdQuery, Result<List<GetRequisitionLogsDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<Result<List<GetRequisitionLogsDto>>> Handle(GetRequisitionLogsByIdQuery request, CancellationToken cancellationToken)
    {
        var logs = await _reqRepository.GetRequisitionLogsById(request.ReqCode);

        if (logs == null || logs.Count == 0)
            return Result<List<GetRequisitionLogsDto>>.Failure(RequisitionErrors.RequisitionLogEmpty(request.ReqCode));

        var result = _mapper.Map<List<GetRequisitionLogsDto>>(logs);
        return Result<List<GetRequisitionLogsDto>>.Success(result);
    }
}