using FluentValidation;
using Application.Features.Common.Validators;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandValidator : AbstractValidator<CreateRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public CreateRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty();

        RuleFor(r => r.Name).RequiredString();

        RuleFor(r => r.StatusId).RequiredForeignKey();

        RuleFor(r => r.WorkTypeId).RequiredForeignKey();

        RuleFor(r => r.HardwareId).RequiredForeignKey();

        RuleFor(r => r.Headcount).RequiredForeignKey();

        RuleFor(r => r.Location).RequiredString();

        RuleFor(r => r.Department).RequiredString();

        RuleFor(r => r.HiringManager).RequiredString();

        RuleFor(r => r.Owner).RequiredString();

        RuleFor(r => r.CompensationBandMin)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(0, int.MaxValue);
        _reqRepository = reqRepository;

        RuleFor(r => r.CompensationBandMax)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(0, int.MaxValue);
        _reqRepository = reqRepository;

        RuleFor(r => r.CompensationBandCurrency)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(3);

        RuleFor(r => r.CompensationBandInterval).RequiredString(10);

        RuleFor(r => r.Description)
            .MaximumLength(200);

        RuleFor(r => r.BackfillName)
            .MaximumLength(100);
    }
}