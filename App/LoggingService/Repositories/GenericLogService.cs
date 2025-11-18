using Application.Interfaces.LoggingService;
using Domain.Common;
using Persistence;

namespace LoggingService.Repositories;
public class GenericLogService<TLog>(AppDbContext context) : IGenericLogService<TLog> where TLog : BaseEntityLog, new()
{
    private readonly AppDbContext _context = context;

    public async Task LogSuccessAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult)
    {
        var log = new TLog
        {
            EntityType = typeof(TLog).Name.Replace("Log",""),
            EntityId = entityId,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TimeStamp = DateTime.UtcNow,
            ActionResult = "Success"
        };

        _context.Set<TLog>().Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task LogFailureAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage, string errorDescription)
    {
        var log = new TLog
        {
            EntityType = typeof(TLog).Name.Replace("Log", ""),
            EntityId = entityId,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TimeStamp = DateTime.UtcNow,
            ActionResult = "Failure",
            ErrorMessage = errorMessage,
            ErrorDescription = errorDescription
        };

        _context.Set<TLog>().Add(log);
        await _context.SaveChangesAsync();
    }
}