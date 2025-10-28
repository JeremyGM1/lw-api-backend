using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggingService.Configuration.RequisitionConfig;
public class RequisitionLogConfig : IEntityTypeConfiguration<RequisitionLog>
{
    public void Configure(EntityTypeBuilder<RequisitionLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.RequisitionId);
        builder.HasIndex(e => e.TimeStamp);

        builder.HasOne(e => e.Requisition)
            .WithMany()
            .HasForeignKey(e => e.RequisitionId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}