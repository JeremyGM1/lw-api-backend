using FluentValidation;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandValidator : AbstractValidator<DeleteRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public DeleteRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MaximumLength(6);

        _reqRepository = reqRepository;
    }
}