namespace Application.Interfaces.LoggingService;
public interface IJobDescriptionLoggingService
{
    Task LogSuccessAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string requisitionId, string title);

    Task LogFailureAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage, string errorDescription, string requisitionId, string title);
}