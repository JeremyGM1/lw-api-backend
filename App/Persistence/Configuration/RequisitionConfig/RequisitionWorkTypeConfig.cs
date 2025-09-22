using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.RequisitionConfig;
public class RequisitionWorkTypeConfig : IEntityTypeConfiguration<RequisitionWorkType>
{
    public void Configure(EntityTypeBuilder<RequisitionWorkType> builder)
    {
        builder.HasKey(wt => wt.Id);
        builder.Property(wt => wt.WorkType).IsRequired();

        builder.HasData(
            new RequisitionWorkType { Id = 1, WorkType = "Contractor"},
            new RequisitionWorkType { Id = 2, WorkType = "FullTime"},
            new RequisitionWorkType { Id = 3, WorkType = "Internship"},
            new RequisitionWorkType { Id = 4, WorkType = "PartTime"}
        );
    }
}