using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using Persistence;

namespace LoggingService.Repositories;
public class GenericLogService(AppDbContext context) : IGenericLogService
{
    private readonly AppDbContext _context = context;

    public async Task LogSuccessAsync(string reqCode, string action, string userEmail, string ipAddress, string userAgent, string actionResult)
    {
        var log = new RequisitionLog
        {
            EntityType = nameof(Requisition),
            EntityId = reqCode,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ActionResult = "Success"
        };

        _context.RequisitionLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task LogFailureAsync(string reqCode, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage)
    {
        var log = new RequisitionLog
        {
            EntityType = nameof(Requisition),
            EntityId = reqCode,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ActionResult = "Failure",
            ErrorMessage = errorMessage
        };

        _context.RequisitionLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}