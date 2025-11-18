using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDsByReqCodeQuery;
/*public class GetJDsByReqCodeQueryHandler(IMapper mapper, IJobDescriptionRepository jdRepository) : IRequestHandler<GetJDsByReqCodeQuery, Result<GetJobDescriptionResponseDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository= jdRepository;
    public async Task<Result<GetJobDescriptionResponseDto>> Handle(GetJDsByReqCodeQuery request, CancellationToken cancellationToken)
    {
        var JobDescription = await _jdRepository.GetJobDescriptionById(request.ReqCode);
    }
}*/