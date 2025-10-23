using Domain.Entities.Requisitions;
using LoggingService.Interfaces;

namespace Persistence.Repositories.LoggingService;

public class RequisitionLogService(AppDbContext context) : IRequisitionLogService
{
    private readonly AppDbContext _context = context;

    public async Task LogAsync(string requisitionId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage)
    {
        var log = new RequisitionLog
        {
            RequisitionId = requisitionId,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ActionResult = actionResult,
            ErrorMessage = errorMessage
        };

        _context.RequisitionLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}