using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace LoggingService;
public partial class LoggingServiceDbContext : DbContext
{
    public required DbSet<RequisitionLog> RequisitionLogs { get; set; }
}