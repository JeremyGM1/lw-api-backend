using Application.Interfaces.LoggingService;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware;

//public class GlobalExceptionHandler : IExceptionHandler
//{
//    private readonly ILoggerService _logger;

//    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//    {
//        _logger.LogFailureAsync(exception, "Exception occurred: {Message}", exception.Message);


//    }
//}
