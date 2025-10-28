using FluentValidation;
using Application.Common.Configs;
using Application.Common.Validators;
using Application.Interfaces;
using Domain.Entities.Requisitions;

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

        RuleFor(r => r.LocationId)
            .MustAsync(LocationExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.Department).NotRequiredString();

        RuleFor(r => r.HiringManager).NotRequiredString();

        RuleFor(r => r.Owner).NotRequiredString();

        RuleFor(r => r.CompensationBandMin)
            .InclusiveBetween(0, ValidationDefaultValues.CompensationBandMaxValue);

        RuleFor(r => r.CompensationBandMax)
            .InclusiveBetween(0, ValidationDefaultValues.CompensationBandMaxValue);

        RuleFor(r => r.CompensationBandCurrencyId)
            .MustAsync(CurrencyExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.CompensationBandIntervalId)
            .MustAsync(IntervalExist).WithMessage("{PropertyName} does not exist")
            .RequiredForeignKey();

        RuleFor(r => r.Description)
            .MaximumLength(200);

        RuleFor(r => r.BackfillName)
            .MaximumLength(100);

        _reqRepository = reqRepository;
    }

    private async Task<bool> StatusExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionStatus>(id);

    private async Task<bool> WorkTypeExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionWorkType>(id);

    private async Task<bool> HardwareExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionHardware>(id);

    private async Task<bool> LocationExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionLocation>(id);

    private async Task<bool> CurrencyExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionCurrency>(id);

    private async Task<bool> IntervalExist(int id, CancellationToken cancellationToken) => await _reqRepository.ForeignKeyExist<RequisitionInterval>(id);

    private async Task<bool> RequisitionExist(string id, CancellationToken cancellationToken) => await _reqRepository.RequisitionExists(id);
}