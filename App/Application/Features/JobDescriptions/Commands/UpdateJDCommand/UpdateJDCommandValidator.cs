using Application.Common.Validators;
using Application.Interfaces.Features.JobDescriptions;
using FluentValidation;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;
public class UpdateJDCommandValidator : AbstractValidator<UpdateJDCommand>
{
    private readonly IJobDescriptionRepository _jdRepository;

    public UpdateJDCommandValidator(IJobDescriptionRepository jdRepository)
    {
        RuleFor(r => r.Title)
            .NotRequiredString();

        RuleFor(r => r.PostingUrl)
            .NotRequiredString();

        RuleFor(r => r.KeyResponsabilities)
            .NotRequiredString();

        RuleFor(r => r.InternalNotes)
            .NotRequiredString();

        _jdRepository = jdRepository;
    }
}