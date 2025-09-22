using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;
public class RequisitionHardwareConfig : IEntityTypeConfiguration<RequisitionHardware>
{
    public void Configure(EntityTypeBuilder<RequisitionHardware> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Hardware).IsRequired();

        builder.HasData(
            new RequisitionHardware { Id = 1, Hardware = "Dell" },
            new RequisitionHardware { Id = 2, Hardware = "Mac" }
        );
    }
}