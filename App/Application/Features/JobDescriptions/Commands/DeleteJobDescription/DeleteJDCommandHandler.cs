using Domain.Entities.JobDescriptions;
using MediatR;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.JobDescriptionErrors;
using Application.Common.Results;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.JobDescriptions;
using Application.Interfaces.LoggingService;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.JobDescriptions.Commands.DeleteJobDescription;

public class DeleteJDCommandHandler(IJobDescriptionRepository jdrepository, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IJobDescriptionLoggingService logger) : IRequestHandler<DeleteJDCommand, Result>
{
    private readonly IJobDescriptionRepository _jdRepository = jdrepository;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IJobDescriptionLoggingService _logger = logger;

    public async Task<Result> Handle(DeleteJDCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();
        var jobDescription = await _jdRepository.GetByIdAsync(request.Id);

        if (jobDescription == null)
        {
            var reqError = JobDescriptionErros.NotExistingJobDescriptionById(request.Id);
            await _logger.LogFailureAsync("N/A", "Delete", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description, "N/A", "N/A");
            return Result.Failure(JobDescriptionErros.NotExistingJobDescriptionById(request.Id));
        }

        //var requisition = await _reqRepository.GetRequisitionById(jobDescription.RequisitionCode);

        //requisition.RemoveJobDescription(jobDescription);

        await _jdRepository.DeleteAsync(jobDescription);

        await _logger.LogSuccessAsync(request.Id.ToString(), "Delete", userEmail, ipAddress, userAgent, "Success", jobDescription.ReqCode, jobDescription.Title);

        return Result.Success();
    }
}