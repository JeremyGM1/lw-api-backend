using Application.Common.Results;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using Domain.Entities.JobDescriptions;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;

public class GetJDsByRequisitionQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetJDsByRequisitionQuery, Result<IEnumerable<GetJobDescriptionResponseDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;

    public async Task<Result<IEnumerable<GetJobDescriptionResponseDto>>> Handle(GetJDsByRequisitionQuery request, CancellationToken cancellationToken)
    {
        var jobDescription = await _jdRepository.GetJobDescriptionsByRequisition(request.ReqCode);

        var result = _mapper.Map<IEnumerable<GetJobDescriptionResponseDto>>(jobDescription);
        return Result<IEnumerable<GetJobDescriptionResponseDto>>.Success(result);
    }
}