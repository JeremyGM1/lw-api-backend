namespace Application.Exceptions;
public abstract class ApplicationException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; protected set; } = statusCode;
}