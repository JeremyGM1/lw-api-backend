using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public partial class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequisitionLog>().Ignore(e => e.Requisition);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}