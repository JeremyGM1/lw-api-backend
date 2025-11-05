using Application.Interfaces.LoggingService;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILoggerService _logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        if (exception is FluentValidation.ValidationException fluentException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = fluentException.Errors.Select(e => e.ErrorMessage).ToList();

            var response = new
            {
                isSuccess = false,
                error = new
                {
                    code = "VALIDATION_ERROR",
                    message = string.Join(", ", errors)
                }
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                isSuccess = false,
                error = new
                {
                    code = "INTERNAL_ERROR",
                    message = "An unexpected error occurred"
                }
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }

        return true;
    }
}