using FluentValidation;
using Application.Common.Validators;
using Application.Interfaces.Features.JobDescriptions;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;

public class UpdateJDCommandValidator : AbstractValidator<UpdateJDCommand>
{
    private readonly IJobDescriptionRepository _jdRepository;
    private readonly IRequisitionRepository _reqRepository;

    public UpdateJDCommandValidator(IJobDescriptionRepository jdRepository, IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.ReqCode)
            .Length(6)
            .MustAsync(BeValidRequisitionCode).WithMessage("The provided {PropertyName} does not exist in the Requisitions Table")
            .When(x => !string.IsNullOrEmpty(x.ReqCode));

        RuleFor(r => r.Title)
            .NotRequiredString(100);

        RuleFor(r => r.PostingUrl)
            .NotRequiredString(500);

        RuleFor(r => r.KeyResponsabilities)
            .NotRequiredString(500);

        RuleFor(r => r.InternalNotes)
            .NotRequiredString(500);

        _jdRepository = jdRepository;
        _reqRepository = reqRepository;
    }
    private async Task<bool> BeValidRequisitionCode(string reqCode, CancellationToken cancellationToken)
    {
        var requisitionExist = await _reqRepository.RequisitionExists(reqCode);
        return requisitionExist;
    }
}