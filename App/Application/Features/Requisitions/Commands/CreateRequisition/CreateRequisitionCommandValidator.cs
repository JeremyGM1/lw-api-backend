using Application.Interfaces;
using FluentValidation;

namespace Application.Features.Requisitions.Commands.CreateRequisition;
public class CreateRequisitionCommandValidator : AbstractValidator<CreateRequisitionCommand>
{
    private readonly IRequisitionRepository _reqRepository;

    public CreateRequisitionCommandValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.Id)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty();


        _reqRepository = reqRepository;
    }
}