using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;

namespace Persistence.Configuration.RequisitionConfig;
public class RequisitionLogConfig : IEntityTypeConfiguration<RequisitionLog>
{
    public void Configure(EntityTypeBuilder<RequisitionLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.TimeStamp);

        //builder.HasOne(e => e.Requisition)
        //    .WithMany()
        //    .HasForeignKey(e => e.EntityId)
        //    .HasPrincipalKey(e => e.ReqCode)
        //    .OnDelete(DeleteBehavior.SetNull)
        //    .IsRequired(false);
    }
}