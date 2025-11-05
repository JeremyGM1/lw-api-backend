namespace Application.Common.Results;
public sealed record Error(string Code, string Description, Dictionary<string, string[]> Metadata = null!)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}