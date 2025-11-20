using FluentValidation;
using Application.Common.Validators;
using Application.Interfaces.Features.JobDescriptions;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;

public class UpdateJDCommandValidator : AbstractValidator<UpdateJDCommand>
{
    private readonly IJobDescriptionRepository _jdRepository;

    public UpdateJDCommandValidator(IJobDescriptionRepository jdRepository)
    {
        RuleFor(r => r.Title)
            .NotRequiredString(100);

        RuleFor(r => r.PostingUrl)
            .NotRequiredString(500);

        RuleFor(r => r.KeyResponsabilities)
            .NotRequiredString(500);

        RuleFor(r => r.InternalNotes)
            .NotRequiredString(500);

        _jdRepository = jdRepository;
    }
}