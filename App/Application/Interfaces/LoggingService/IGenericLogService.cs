using Domain.Common;

namespace Application.Interfaces.LoggingService;

public interface IGenericLogService<TLog> where TLog : BaseEntityLog
{
    Task LogSuccessAsync(string EntityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult);

    Task LogFailureAsync(string EntityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage, string errorDescription);
}