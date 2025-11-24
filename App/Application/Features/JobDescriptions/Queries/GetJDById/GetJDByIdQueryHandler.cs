using Application.Common.Results;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using Domain.Entities.JobDescriptions;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDById;
public class GetJDByIdQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetJDByIdQuery, Result<GetJobDescriptionResponseDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    public async Task<Result<GetJobDescriptionResponseDto>> Handle(GetJDByIdQuery request, CancellationToken cancellationToken)
    {
        var jobDescription = await _jdRepository.GetJobDescriptionById(request.Id);

        if (jobDescription == null)
            return Result<GetJobDescriptionResponseDto>.Failure(JobDescriptionErros.NotExistingJobDescriptionById(request.Id));

        var result = _mapper.Map<GetJobDescriptionResponseDto>(jobDescription);
        return Result<GetJobDescriptionResponseDto>.Success(result);
    }
}