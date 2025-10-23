using LoggingService.Interfaces;

namespace Persistence.Repositories.LoggingService;

public class RequisitionLogService(AppDbContext context) : GenericLogService(context), IRequisitionLogService
{
}