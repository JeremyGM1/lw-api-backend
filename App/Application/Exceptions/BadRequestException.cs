using FluentValidation.Results;

namespace Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException() : base()
    {
    }

    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationErrors = validationResult.ToDictionary();
    }

    public BadRequestException(string message, IEnumerable<string> errors) : base(message)
    {
        ValidationErrors = new Dictionary<string, string[]>
        {
            {"General", errors.ToArray() }
        };
    }

    public IDictionary<string, string[]> ValidationErrors { get; } = new Dictionary<string, string[]>();
}