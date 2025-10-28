using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace LoggingService;
public partial class LoggingServiceDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequisitionLog>().Ignore(e => e.Requisition);
    }
}