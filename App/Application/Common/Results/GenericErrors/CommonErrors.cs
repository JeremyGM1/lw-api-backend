using Application.Common.Results;

namespace Application.Common.Results.GenericErrors;
public static class CommonErrors
{
    public static Error InvalidForeignKey(Dictionary<string, string[]> errors) => new(
        "Common.ForeignKey",
        "Invalid foreign key",
        errors);
}