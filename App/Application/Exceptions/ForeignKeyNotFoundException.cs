namespace Application.Exceptions;

public class ForeignKeyNotFoundException : Exception
{
    public IDictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();

    public ForeignKeyNotFoundException() : base()
    {
    }

    public ForeignKeyNotFoundException(string? message) : base(message)
    {
    }

    public ForeignKeyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ForeignKeyNotFoundException(string message, IDictionary<string, string[]> validationErrors) : base(message)
    {
        ValidationErrors = validationErrors;
    }
}