using FluentValidation;

namespace Application.Features.Common.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> RequiredString<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int maxLength = 100)
    {
        return ruleBuilder
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(maxLength);
    }

    public static IRuleBuilderOptions<T, int> RequiredForeignKey<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int maxValue = int.MaxValue)
    {
        return ruleBuilder
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(1, maxValue);
    }
}