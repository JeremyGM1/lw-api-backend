namespace Application.Common.Results.GenericErrors;

public static class ValidationErrors
{
    public static Error ValidationFailed(Dictionary<string, string[]> errors) => new(
        "Validation.ValidationFailed",
        "One or more validation errors occurred",
        errors);

    public static Error InvalidInput(string message) => new(
        "Validation.InvalidInput",
        message);

    public static Error RequiredField(string fieldName) => new(
        "Validation.RequiredField",
        $"{fieldName} is required");
}