using Application.Common.Results;

namespace Application.Common.Results.GenericErrors;
public static class ValidationErrors
{
    public static Error ValidationFailed(Dictionary<string, string[]> errors) => new(
        "Validation.ValidationFailed",
        "One or more validation errors occurred",
        errors);
}