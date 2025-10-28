using FluentValidation;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandValidator : AbstractValidator<DeleteRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public DeleteRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(6)
            .MustAsync(RequisitionExist).WithMessage("Requisition with {PropertyName} does not exist");

        _reqRepository = reqRepository;
    }

    private async Task<bool> RequisitionExist(string id, CancellationToken cancellationToken) => await _reqRepository.RequisitionExists(id);
}