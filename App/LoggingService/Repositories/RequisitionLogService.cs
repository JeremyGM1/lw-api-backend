using Application.Interfaces.LoggingService;
using Persistence;

namespace LoggingService.Repositories;

public class RequisitionLogService(AppDbContext context) : GenericLogService(context), IRequisitionLogService;