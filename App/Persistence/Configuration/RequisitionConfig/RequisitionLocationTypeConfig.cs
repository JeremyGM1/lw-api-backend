using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.RequisitionConfig;

public class RequisitionLocationTypeConfig : IEntityTypeConfiguration<RequisitionLocationType>
{
    public void Configure(EntityTypeBuilder<RequisitionLocationType> builder)
    {
        builder.HasKey(wt => wt.Id);
        builder.Property(wt => wt.LocationType).IsRequired();

        builder.HasData(
            new RequisitionLocationType { Id = 1, LocationType = "On-Site" },
            new RequisitionLocationType { Id = 2, LocationType = "Hybrid" },
            new RequisitionLocationType { Id = 3, LocationType = "Virtual" }
        );
    }
}