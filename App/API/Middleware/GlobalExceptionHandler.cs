using Application.Interfaces.LoggingService;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        if (exception is FluentValidation.ValidationException fluentException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = fluentException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToList()
                );

            var response = new
            {
                isSuccess = false,
                error = new
                {
                    code = "VALIDATION_ERROR",
                    description = "One or more validation errors occurred.",
                    metadata = errors
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
                    description = "One or more validation errors occurred.",
                    metadata = (object)null!
                }
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        }

        return true;
    }
}