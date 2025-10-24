using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;

public class RequisitionLocationConfig : IEntityTypeConfiguration<RequisitionLocation>
{
    public void Configure(EntityTypeBuilder<RequisitionLocation> builder)
    {
        builder.HasKey(wt => wt.Id);
        builder.Property(wt => wt.Location).IsRequired();

        builder.HasData(
            new RequisitionLocation { Id = 1, Location = "Medellín" },
            new RequisitionLocation { Id = 2, Location = "Bogotá" },
            new RequisitionLocation { Id = 3, Location = "Cali" },
            new RequisitionLocation { Id = 4, Location = "Barranquilla" },
            new RequisitionLocation { Id = 5, Location = "Cartagena" },
            new RequisitionLocation { Id = 6, Location = "Bucaramanga" },
            new RequisitionLocation { Id = 7, Location = "Santa Marta" }
        );
    }
}