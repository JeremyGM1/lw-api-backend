using FluentValidation;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandValidator : AbstractValidator<DeleteRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public DeleteRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.ReqCode)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(6);

        _reqRepository = reqRepository;
    }
}