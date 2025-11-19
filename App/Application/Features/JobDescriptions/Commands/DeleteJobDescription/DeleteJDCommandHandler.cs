using Application.Common.Results;
using Application.Common.Results.JobDescriptionErrors;
using Application.Interfaces.Features.JobDescriptions;
using MediatR;

namespace Application.Features.JobDescriptions.Commands.DeleteJobDescription;
public class DeleteJDCommandHandler(IJobDescriptionRepository jdrepository) : IRequestHandler<DeleteJDCommand, Result>
{
    private readonly IJobDescriptionRepository _jdRepository = jdrepository;
    public async Task<Result> Handle(DeleteJDCommand request, CancellationToken cancellationToken)
    {
        var jobDescription = await _jdRepository.GetByIdAsync(request.Id);

        if (jobDescription == null)
            return Result.Failure(JobDescriptionErros.NotExistingJobDescription(request.Id));

        await _jdRepository.DeleteAsync(jobDescription);

        return Result.Success();
    }
}