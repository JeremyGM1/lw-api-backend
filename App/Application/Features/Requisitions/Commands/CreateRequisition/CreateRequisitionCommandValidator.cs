using Application.Common.Validators;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using FluentValidation;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandValidator : AbstractValidator<CreateRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public CreateRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(6);

        RuleFor(r => r.Name).RequiredString();

        RuleFor(r => r.StatusId)
            .MustAsync(StatusExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.WorkTypeId)
            .MustAsync(WorkTypeExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.HardwareId)
            .MustAsync(HardwareExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.Headcount).RequiredInt();

        RuleFor(r => r.Location).RequiredString();

        RuleFor(r => r.Department).RequiredString();

        RuleFor(r => r.HiringManager).RequiredString();

        RuleFor(r => r.Owner).RequiredString();

        RuleFor(r => r.CompensationBandMin)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(0, int.MaxValue);

        RuleFor(r => r.CompensationBandMax)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(0, int.MaxValue);

        RuleFor(r => r.CompensationBandCurrency).RequiredString(3);

        RuleFor(r => r.CompensationBandInterval).RequiredString(10);

        RuleFor(r => r.Description)
            .MaximumLength(200);

        RuleFor(r => r.BackfillName)
            .MaximumLength(100);

        RuleFor(r => r.Id)
            .MustAsync(EntityNotExist).WithMessage("Requisition with this {PropertyName} already exist.");

        _reqRepository = reqRepository;
    }

    private async Task<bool> StatusExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionStatus>(id);

    private async Task<bool> WorkTypeExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionWorkType>(id);

    private async Task<bool> HardwareExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionHardware>(id);

    private async Task<bool> EntityNotExist(string id, CancellationToken cancellationToken) => !await _reqRepository.RequisitionExists(id);
}