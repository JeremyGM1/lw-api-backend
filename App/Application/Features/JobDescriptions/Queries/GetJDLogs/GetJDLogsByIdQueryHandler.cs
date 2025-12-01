using Application.Common.Results;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDLogs;
public class GetJDLogsByIdQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetJDLogsByIdQuery, Result<List<GetJobDescriptionLogsResponseDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    public async Task<Result<List<GetJobDescriptionLogsResponseDto>>> Handle(GetJDLogsByIdQuery request, CancellationToken cancellationToken)
    {
        var logs = await _jdRepository.GetJDLogsById(request.id);

        if (logs == null || logs.Count == 0)
            return Result<List<GetJobDescriptionLogsResponseDto>>.Failure(CommonErrors.EmptyLogList(request.id.ToString()));

        var result = _mapper.Map<List<GetJobDescriptionLogsResponseDto>>(logs);

        return Result<List<GetJobDescriptionLogsResponseDto>>.Success(result);
    }
}