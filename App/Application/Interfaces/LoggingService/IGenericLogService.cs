namespace Application.Interfaces.LoggingService;

public interface IGenericLogService
{
    Task LogSuccessAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult);

    Task LogFailureAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage);
}