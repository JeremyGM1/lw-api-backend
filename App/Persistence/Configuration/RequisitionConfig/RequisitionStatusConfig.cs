using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;

public class RequisitionStatusConfig : IEntityTypeConfiguration<RequisitionStatus>
{
    public void Configure(EntityTypeBuilder<RequisitionStatus> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Status).IsRequired();

        builder.HasData(
            new RequisitionStatus { Id = 1, Status = "Cancelled" },
            new RequisitionStatus { Id = 2, Status = "Closed" },
            new RequisitionStatus { Id = 3, Status = "Completed" },
            new RequisitionStatus { Id = 4, Status = "Open" }
        );
    }
}