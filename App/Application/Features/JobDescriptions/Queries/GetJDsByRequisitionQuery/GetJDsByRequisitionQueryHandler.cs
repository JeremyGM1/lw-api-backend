using Application.Common.Results;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using Domain.Entities.JobDescriptions;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;

public class GetJDsByRequisitionQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetJDsByRequisitionQuery, Result<List<GetJobDescriptionResponseDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;

    public async Task<Result<List<GetJobDescriptionResponseDto>>> Handle(GetJDsByRequisitionQuery request, CancellationToken cancellationToken)
    {
        var jobDescription = await _jdRepository.GetJobDescriptionsByRequisition(request.ReqCode);

        if (jobDescription == null || jobDescription.Count == 0)
            return Result<List<GetJobDescriptionResponseDto>>.Failure(JobDescriptionErros.NotExistingJobDescriptionByRequisitionCode(request.ReqCode));

        var result = _mapper.Map<List<GetJobDescriptionResponseDto>>(jobDescription);
        return Result<List<GetJobDescriptionResponseDto>>.Success(result);
    }
}