using Application.Interfaces.Features.JobDescriptions;
using FluentValidation;

namespace Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;
public class GetJDsByRequisitionQueryValidator : AbstractValidator<GetJDsByRequisitionQuery>
{
    private readonly IJobDescriptionRepository _jdRepository;

    public GetJDsByRequisitionQueryValidator(IJobDescriptionRepository jdRepository)
    {
        RuleFor(r => r.ReqCode)
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .Length(6);

        _jdRepository = jdRepository;
    }
}