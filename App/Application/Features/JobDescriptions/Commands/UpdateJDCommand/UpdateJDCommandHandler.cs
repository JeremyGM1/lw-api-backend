using Application.Common.Results;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.JobDescriptionErrors;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.JobDescriptions;
using Application.Interfaces.LoggingService;
using AutoMapper;
using MediatR;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;
public class UpdateJDCommandHandler(IMapper mapper,IJobDescriptionRepository jdRepository, ICommonHelpers commonHelpers, IJobDescriptionLoggingService logger) : IRequestHandler<UpdateJDCommand, Result<UpdateJobDescriptionResponseDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IJobDescriptionLoggingService _logger = logger;
    public async Task<Result<UpdateJobDescriptionResponseDto>> Handle(UpdateJDCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
            var jobDescription = await _jdRepository.GetJobDescriptionById(request.Id);

            if (jobDescription == null)
            {
                var reqError = JobDescriptionErros.NotExistingJobDescriptionById(request.Id);
                await _logger.LogFailureAsync("N/A", "Update", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description, "N/A", "N/A");
                return Result<UpdateJobDescriptionResponseDto>.Failure(JobDescriptionErros.NotExistingJobDescriptionById(request.Id));
            }

            _mapper.Map(request, jobDescription);

            jobDescription.UpdatedAt = DateTime.UtcNow;
            jobDescription.UpdatedBy = userEmail;

            await _jdRepository.UpdateAsync(jobDescription);

            var response = _mapper.Map<UpdateJobDescriptionResponseDto>(jobDescription);

            await _logger.LogSuccessAsync(request.Id.ToString(), "Update", userEmail, ipAddress, userAgent, "Success", jobDescription.ReqCode, jobDescription.Title);
            return Result<UpdateJobDescriptionResponseDto>.Success(response);
        }
        catch(Exception ex)
        {
            var errorDescription = $"{ex.Message}" + (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");
            await _logger.LogFailureAsync("N/A", "Update", userEmail, ipAddress, userAgent, "Failure", ex.Message, errorDescription, "N/A", "N/A");
            return Result<UpdateJobDescriptionResponseDto>.Failure(CommonErrors.UnexpectedException(errorDescription));
        }
    }
}
