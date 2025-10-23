namespace Application.Common.Helpers;
public interface ILoggerHelpers
{
    Task LogRequisitionAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string? errorMessage = null);
}