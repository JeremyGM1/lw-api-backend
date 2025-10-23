using LoggingService.Interfaces;

namespace Application.Common.Helpers;
public class LoggerHelpers(IRequisitionLogService logger) : ILoggerHelpers
{
    private readonly IRequisitionLogService _logger = logger;
    public async Task LogRequisitionAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string? errorMessage = null)
    {
        await _logger.LogAsync(
            requisitionId,
            action,
            userEmail,
            ipAddress,
            userAgent,
            actionResult,
            errorMessage!
        );
    }
}