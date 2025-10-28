using Application.Interfaces.Features.Common;
using Application.Interfaces.LoggingService;

namespace API.Middleware;

public class ValidationLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 400)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<IRequisitionLogService>();
            var contextHelper = scope.ServiceProvider.GetRequiredService<ICommonHelpers>();

            var (userEmail, ipAddress, userAgent) = contextHelper.GetLoggingContext();

            await logger.LogFailureAsync(
                null!,
                "ValidationError",
                userEmail,
                ipAddress,
                userAgent,
                "Failure",
                "Validation Failed"
            );
        }
    }
}