using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetAllJDsQuery;
public class GetAllJDsQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetAllJDsQuery, Result<List<GetJobDescriptionResponseDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    public async Task<Result<List<GetJobDescriptionResponseDto>>> Handle(GetAllJDsQuery request, CancellationToken cancellationToken)
    {
        var jds = await _jdRepository.GetJobDescriptions();

        var data = _mapper.Map<List<GetJobDescriptionResponseDto>>(jds);

        return Result<List<GetJobDescriptionResponseDto>>.Success(data);
    }
}