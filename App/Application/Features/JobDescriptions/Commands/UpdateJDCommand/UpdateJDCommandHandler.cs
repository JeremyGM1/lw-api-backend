using Application.Common.Results;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Request;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.JobDescriptions;
using AutoMapper;
using MediatR;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;
public class UpdateJDCommandHandler(IMapper mapper,IJobDescriptionRepository jdRepository, ICommonHelpers commonHelpers) : IRequestHandler<UpdateJDCommand, Result<UpdateJobDescriptionResponseDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    public async Task<Result<UpdateJobDescriptionResponseDto>> Handle(UpdateJDCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        var jd = await _jdRepository.GetByIdAsync(request.Id);

        if (jd == null)
            return Result<UpdateJobDescriptionResponseDto>.Failure(JobDescriptionErros.NotExistingJobDescription(request.Id));

        _mapper.Map(request, jd);

        jd.UpdatedAt = DateTime.UtcNow;
        jd.UpdatedBy = userEmail;

        await _jdRepository.UpdateAsync(jd);

        var UpdatedJobDescription = await _jdRepository.GetByIdAsync(jd.Id);

        var response = _mapper.Map<UpdateJobDescriptionResponseDto>(UpdatedJobDescription);
        return Result<UpdateJobDescriptionResponseDto>.Success(response);
    }
}
