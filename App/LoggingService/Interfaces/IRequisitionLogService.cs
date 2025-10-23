namespace LoggingService.Interfaces;
public interface IRequisitionLogService
{
    Task LogAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage);
}