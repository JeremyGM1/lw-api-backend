namespace Application.Common.Results.GenericErrors;

public static class CommonErrors
{
    public static Error InvalidForeignKey(Dictionary<string, string[]> errors) => new(
        "Common.ForeignKey",
        "Invalid foreign key",
        errors);

    public static Error UnexpectedException(string message) => new(
        "Common.Exception",
        $"An error occurred while processing the request: {message}");

    public static Error EmptyLogList(string id) => new(
        "Common.Empty",
        $"Entity with id '{id}' doesn't have logs");
}