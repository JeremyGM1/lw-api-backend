using Application.Interfaces.LoggingService;

namespace LoggingService.Repositories;

public class RequisitionLogService(LoggingServiceDbContext context) : GenericLogService(context), IRequisitionLogService
{
}