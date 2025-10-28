using LoggingService.Interfaces;

namespace LoggingService.Repositories;

public class RequisitionLogService(LoggingServiceDbContext context) : GenericLogService(context), IRequisitionLogService
{
}