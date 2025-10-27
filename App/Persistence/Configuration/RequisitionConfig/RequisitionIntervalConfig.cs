using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;

public class RequisitionIntervalConfig : IEntityTypeConfiguration<RequisitionInterval>
{
    public void Configure(EntityTypeBuilder<RequisitionInterval> builder)
    {
        builder.HasKey(wt => wt.Id);
        builder.Property(wt => wt.Interval).IsRequired();

        builder.HasData(
            new RequisitionInterval { Id = 1, Interval = "Weekly" },
            new RequisitionInterval { Id = 2, Interval = "Bi-Weekly" },
            new RequisitionInterval { Id = 3, Interval = "Monthly" },
            new RequisitionInterval { Id = 4, Interval = "Bi-Monthly" },
            new RequisitionInterval { Id = 5, Interval = "Quarterly" },
            new RequisitionInterval { Id = 6, Interval = "Bi-Annually" }
        );
    }
}