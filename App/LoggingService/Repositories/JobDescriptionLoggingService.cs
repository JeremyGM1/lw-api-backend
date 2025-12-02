using Application.Interfaces.LoggingService;
using Domain.Entities.JobDescriptions;
using Domain.Entities.Requisitions;
using Persistence;

namespace LoggingService.Repositories;
public class JobDescriptionLoggingService(AppDbContext context) : IJobDescriptionLoggingService
{
    private readonly AppDbContext _context = context;
    public async Task LogFailureAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string errorMessage, string errorDescription, string reqCode, string title)
    {
        var log = new JobDescriptionLog
        {
            EntityType = typeof(JobDescription).Name,
            EntityId = entityId,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TimeStamp = DateTime.UtcNow,
            ActionResult = "Failure",
            ErrorMessage = errorMessage,
            ErrorDescription = errorDescription,
            ReqCode = reqCode,
            Title = title
        };

        _context.Set<JobDescriptionLog>().Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task LogSuccessAsync(string entityId, string action, string userEmail, string ipAddress, string userAgent, string actionResult, string reqCode, string title)
    {
        var log = new JobDescriptionLog
        {
            EntityType = typeof(JobDescription).Name,
            EntityId = entityId,
            Action = action,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TimeStamp = DateTime.UtcNow,
            ActionResult = "Success",
            ReqCode = reqCode,
            Title = title
        };

        _context.Set<JobDescriptionLog>().Add(log);
        await _context.SaveChangesAsync();
    }
}