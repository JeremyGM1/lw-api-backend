using FluentValidation;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryValidator : AbstractValidator<GetRequisitionByIdQuery>
{
    private readonly IRequisitionRepository _reqRepository;

    public GetRequisitionByIdQueryValidator(IRequisitionRepository reqRepository)
    {
        RuleFor(r => r.ReqCode)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(6);

        RuleFor(r => r.ReqCode)
            .MustAsync(EntityExist)
            .WithMessage("Requisition with {PropertyName} does not exist");

        _reqRepository = reqRepository;
    }

    private async Task<bool> EntityExist(string id, CancellationToken cancellationToken) => await _reqRepository.RequisitionExists(id);
}