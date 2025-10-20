using FluentValidation;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Application.Common.Validators;
using Application.Common.Configs;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandValidator : AbstractValidator<UpdateRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public UpdateRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .MaximumLength(6)
            .MustAsync(RequisitionExist).WithMessage("Requisition with {PropertyName} does not exist");

        RuleFor(r => r.Name).NotRequiredString();

        RuleFor(r => r.StatusId)
            .MustAsync(StatusExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.WorkTypeId)
            .MustAsync(WorkTypeExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.HardwareId)
            .MustAsync(HardwareExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.Headcount).InclusiveBetween(0, 1000);

        RuleFor(r => r.Location).NotRequiredString();

        RuleFor(r => r.Department).NotRequiredString();

        RuleFor(r => r.HiringManager).NotRequiredString();

        RuleFor(r => r.Owner).NotRequiredString();

        RuleFor(r => r.CompensationBandMin)
            .InclusiveBetween(0, ValidationDefaultValues.CompensationBandMaxValue);

        RuleFor(r => r.CompensationBandMax)
            .InclusiveBetween(0, ValidationDefaultValues.CompensationBandMaxValue);

        RuleFor(r => r.CompensationBandCurrency).NotRequiredString(3);

        RuleFor(r => r.CompensationBandInterval).NotRequiredString(10);

        RuleFor(r => r.Description)
            .MaximumLength(200);

        RuleFor(r => r.BackfillName)
            .MaximumLength(100);

        _reqRepository = reqRepository;
    }

    private async Task<bool> StatusExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionStatus>(id);

    private async Task<bool> WorkTypeExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionWorkType>(id);

    private async Task<bool> HardwareExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionHardware>(id);

    private async Task<bool> RequisitionExist(string id, CancellationToken cancellationToken) => await _reqRepository.RequisitionExists(id);
}