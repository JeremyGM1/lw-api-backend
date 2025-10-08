using Application.Interfaces;
using FluentValidation;

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
            .MaximumLength(6);

        _reqRepository = reqRepository;
    }
}