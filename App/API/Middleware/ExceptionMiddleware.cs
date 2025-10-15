using System.Net;
using API.Middleware.Models;
using Application.Exceptions;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problem;

        switch (ex)
        {
            case BadRequestException BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Title = BadRequestException.Message,
                    Status = (int)statusCode,
                    Detail = BadRequestException.InnerException?.Message,
                    Type = nameof(BadRequestException),
                    Errors = BadRequestException.ValidationErrors
                };
                break;

            case NotFoundException NotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Title = NotFound.Message,
                    Status = (int)statusCode,
                    Detail = NotFound.InnerException?.Message,
                    Type = nameof(NotFound),
                };
                break;

            case UnauthorizedAccessException UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                problem = new CustomProblemDetails
                {
                    Title = UnauthorizedAccessException.Message,
                    Status = (int)statusCode,
                    Detail = UnauthorizedAccessException.InnerException?.Message,
                    Type = nameof(UnauthorizedAccessException)
                };
                break;

            case ForeignKeyNotFoundException ForeignKeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Title = ForeignKeyNotFoundException.Message,
                    Status = (int)statusCode,
                    Detail = ForeignKeyNotFoundException.InnerException?.Message,
                    Type = nameof(ForeignKeyNotFoundException),
                    Errors = ForeignKeyNotFoundException.ValidationErrors
                };
                break;

            default:
                problem = new CustomProblemDetails
                {
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Detail = ex.StackTrace,
                    Type = nameof(HttpStatusCode.InternalServerError),
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}