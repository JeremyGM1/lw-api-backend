using FluentValidation;
using Application.Common.Validators;
using Application.Interfaces.Features.JobDescriptions;

namespace Application.Features.JobDescriptions.Commands.CreateJobDescription;

public class CreateJDCommandValidator : AbstractValidator<CreateJDCommand>
{
    private readonly IJobDescriptionRepository _jdRepository;

    public CreateJDCommandValidator(IJobDescriptionRepository jdRepository)
    {
        RuleFor(r => r.Title)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(100);

        RuleFor(r => r.PostingUrl)
            .RequiredString();

        RuleFor(r => r.KeyResponsabilities)
            .RequiredString();

        RuleFor(r => r.InternalNotes)
            .RequiredString();

        _jdRepository = jdRepository;
    }
}