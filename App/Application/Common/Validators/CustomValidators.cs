using Application.Common.Configs;
using FluentValidation;

namespace Application.Common.Validators;

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

    public static IRuleBuilderOptions<T, string?> NotRequiredString<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int maxLength = 100)
    {
        return ruleBuilder
            .MinimumLength(1)
            .MaximumLength(maxLength);
    }

    public static IRuleBuilderOptions<T, int> RequiredForeignKey<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int maxValue = ValidationDefaultValues.IntMaxValue)
    {
        return ruleBuilder
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(1, maxValue);
    }
    public static IRuleBuilderOptions<T, int> RequiredInt<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int maxValue = ValidationDefaultValues.IntMaxValue)
    {
        return ruleBuilder
            .NotNull().WithMessage("{PropertyName} is required")
            .NotEmpty()
            .InclusiveBetween(0, maxValue);
    }
}